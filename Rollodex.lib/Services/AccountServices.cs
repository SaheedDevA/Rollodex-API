using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Google.Authenticator;
using Rollodex.lib.Models.Entities;
using Rollodex.lib.Models.Request;
using Rollodex.lib.Models.Response;
using Rolodex.Lib.Data;
using Rolodex.Lib.Utils.Authorization;
using Rolodex.Lib.Utils.Helpers;
using Rollodex.lib.Services;
using Rollodex.lib.Models;

namespace Intel.Lib.Services
{
    public interface IAccountServices
    {
        Response<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
        Response<int> CreateWorkSpace(CreateWorkSpaceRequest model);
        Response<int> SetUpWorkSpace(SetupWorkSpaceRequest model);
        Response<string> VerifyToken(VerifyTokendRequest model);
        Response<string> SendToken(string email);
        Response<string> ResetPasswordRequest(ResetPasswordRequest resetPasswordRequest);
        Response<string> ResetPasswordConfirmed(ResetPasswordConfirmed ResetPasswordConfirmed);
        Response<string> ChangePassword(ChangePasswordRequest model);

    }

    public class AccountServices : IAccountServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;
        private EncryptionHelper _encryptionHelper;
        private IHttpContextAccessor _httpContextAccessor;

        public AccountServices(
            ApplicationDbContext context,
            IJwtUtils jwtUtils,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IEmailService emailService,
             IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _encryptionHelper = new EncryptionHelper(_appSettings.Securitykey, _appSettings.SecurityIv);
        }

        public Response<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Email.ToLower() == model.Email.ToLower());

            // validate
            if (account == null || !SecureTextHasher.Verify(model.Password, account.PasswordHash))
            {
                throw new AppException("Invalid login credentials");
            }

            if (account.IsDisabled)
            {
                throw new AppException("Account is disabled, please contact Admin");
            }

            if (account.IsVerified)
            {
                throw new AppException("Account is not verified, please verify your account before login");
            }

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = _jwtUtils.GenerateJwtToken(account);
            var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            account.RefreshTokens.Add(refreshToken);
            account.LastTimeLoggedIn = DateTime.UtcNow;

            // remove old refresh tokens from account
            removeOldRefreshTokens(account);

            // save changes to db
            _context.Update(account);
            _context.SaveChanges();

            var response = new AuthenticateResponse(account, _appSettings.BaseUrl, jwtToken, refreshToken.Token);

            return new Response<AuthenticateResponse>
            {
                Data = response,
                Message = "Login Sucessful",
                Succeeded = true
            };
        }


        public Response<int> CreateWorkSpace(CreateWorkSpaceRequest model)
        {

            //check if password is string
            var isPasswordStrong = SecureTextHasher.IsPasswordStrong(model.Password);
            if (!isPasswordStrong)
            {
                throw new AppException("Password must have minimum of 8 characters in lenght/nAt least one upper case\nAt least one lower case\nAt least one digit\nAt least one special character");
            }

            if (_context.Accounts.Any(x => (x.Email.ToLower() == model.Email)))
            {
                throw new AppException("Your information matches an existing user,\nplease fill in the correct information or sign in");
            }


            // map model to new account object
            var account = new Account
            {
                Email = model.Email,
                UserType = model.UserType,
                Created = DateTime.UtcNow,
                VerificationToken = generateVerificationToken(),
                PasswordHash = SecureTextHasher.Hash(model.Password)
             };

            // save account
            _context.Accounts.Add(account);
            _context.SaveChanges();

            return new Response<int>
            {
                Data = account.Id,
                Succeeded = true,
                Message = "Work space has been created Successful"
            };
        }

        public Response<int> SetUpWorkSpace(SetupWorkSpaceRequest model)
        {
            //check if workpace name/sytem already exist
            var existingWorkspace = _context.Systems.FirstOrDefault(x => x.SystemName.ToLower().Trim()
            == model.WorkSpaceName.ToLower().Trim());

            if(existingWorkspace == null)
            {
                throw new AppException("The name for this workspace already exist");
            }

            //validate and upload base 64 logo

            var admin = _context.Accounts.FirstOrDefault(x => x.Email.Trim().ToLower()
            == model.AdminEmail.Trim().ToLower());

            if (admin == null)
            {
                throw new AppException("Invalid Admin Email");
            }

            RolodexSystem rolodexSystem = new RolodexSystem
            {
                AdminId = admin.Id,
                SystemName = model.WorkSpaceName,
                HasStartedMapping = false,
                ThemeColor = model.Color,
                HasInvitedOrganizations = false,
                HasCreatedDataSet = false,
                WorkSpaceUrl = model.WorkSpaceName,
                CreatedBy = admin.Id,
            };

            _context.Systems.Add(rolodexSystem);
            _context.SaveChanges();

            return new Response<int>
            {
                Data = rolodexSystem.Id,
                Succeeded = true,
                Message = "Work Space set up was Successful"

            };
        }

        public Response<string> SendToken(string email)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Email == email);

            // always return ok response to prevent email enumeration
            if (account == null)
                return new Response<string>
                {
                    Succeeded = false,
                    Message = "Account not valid",
                    Data = null
                };

            // create reset token that expires after 10 mins
            account.ResetToken = generateResetToken();
            account.ResetTokenExpires = DateTime.UtcNow.AddMinutes(10);

            _context.Accounts.Update(account);
            _context.SaveChanges();

            // send email
            sendTokenVerificationEmail(account, account.ResetToken);

            return new Response<string>
            {
                Data = account.ResetToken,
                Message = $"We have sent the Verification token to {account.Email}\nPlease follow the instructions to verifiy your account",
                Succeeded = true
            };

        }

        public Response<string> VerifyToken(VerifyTokendRequest model)
        {
            var account = getAccountByResetToken(model.Token);

            // update password and remove reset token
            account.PasswordReset = DateTime.UtcNow;
            account.ResetToken = null;
            account.ResetTokenExpires = null;


            _context.Accounts.Update(account);
            _context.SaveChanges();

            return new Response<string>
            {
                Data = null,
                Message = "Account Verification Successful,\nYou can now login",
                Succeeded = true
            };
        }


        public Response<string> ResetPasswordRequest(ResetPasswordRequest resetPasswordRequest)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Email == resetPasswordRequest.Email);

            // always return ok response to prevent email enumeration
            if (account == null)
                return new Response<string>
                {
                    Succeeded = false,
                    Message = "Account not valid",
                    Data = null
                };

            // create reset token that expires after 1 day
            account.ResetToken = generateResetToken();
            account.ResetTokenExpires = DateTime.UtcNow.AddMinutes(10);

            _context.Accounts.Update(account);
            _context.SaveChanges();

            // send email
            sendPasswordResetEmail(account, $"{resetPasswordRequest.ResetPasswordUrl}?token={account.ResetToken}");

            return new Response<string>
            {
                Data = account.ResetToken,
                Message = $"We have sent the reset token to {account.Email}\nPlease follow the instructions to reset your password",
                Succeeded = true
            };
        }

        public Response<string> ResetPasswordConfirmed(ResetPasswordConfirmed ResetPasswordConfirmed)
        {
            var account = getAccountByResetToken(ResetPasswordConfirmed.Token);

            // update password and remove reset token
            account.PasswordHash = SecureTextHasher.Hash(ResetPasswordConfirmed.Password);
            account.PasswordReset = DateTime.UtcNow;
            account.ResetToken = null;
            account.ResetTokenExpires = null;

            _context.Accounts.Update(account);
            _context.SaveChanges();

            return new Response<string>
            {
                Data = null,
                Message = "Password Reset Sucessful,\nYou can now login",
                Succeeded = true
            };
        }

        public Response<string> ChangePassword(ChangePasswordRequest model)
        {
            var existingUser = getAccount(model.UserId);
            if (!SecureTextHasher.Verify(model.OldPassword, existingUser.PasswordHash))
                throw new AppException("Invalid old password");

            existingUser.PasswordHash = SecureTextHasher.Hash(model.NewPassword);
            _context.Accounts.Update(existingUser);
            _context.SaveChanges();

            return new Response<string>
            {
                Data = "Password updated successful",
                Message = "Password updated successful",
                Succeeded = true
            };
        }



        public Response<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            var account = getAccountByRefreshToken(token);
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                revokeDescendantRefreshTokens(refreshToken, account, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                _context.Update(account);
                _context.SaveChanges();
            }

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = rotateRefreshToken(refreshToken, ipAddress);
            account.RefreshTokens.Add(newRefreshToken);

            // remove old refresh tokens from account
            removeOldRefreshTokens(account);

            // save changes to db
            _context.Update(account);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = _jwtUtils.GenerateJwtToken(account);

            // return data in authenticate response object
            var response = _mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = newRefreshToken.Token;
            return new Response<AuthenticateResponse>
            {
                Succeeded = true,
                Message = "Sucessful",
                Data = response,
            };
        }

        public Response<bool> RevokeToken(string token, string ipAddress)
        {
            var account = getAccountByRefreshToken(token);
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            // revoke token and save
            revokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
            _context.Update(account);
            _context.SaveChanges();

            return new Response<bool> { Succeeded = true, Message = "Revoke Token SUcessful", Data = true };
        }

        public Response<string> VerifyEmail(string token)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.VerificationToken == token);

            if (account == null)
                throw new AppException("Verification failed");

            account.Verified = DateTime.UtcNow;
            account.VerificationToken = null;

            _context.Accounts.Update(account);
            _context.SaveChanges();

            return new Response<string>
            {
                Data = null,
                Message = "Verification Sucessful",
                Succeeded = true
            };
        }




        #region -- helepr methods
        private Account getAccount(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null) throw new KeyNotFoundException("Admin not found");
            return account;
        }

        private Account getAccountByEmail(string email)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Email == email);
            if (account == null) throw new KeyNotFoundException("Admin not found");
            return account;
        }

        private Account getAccountByRefreshToken(string token)
        {
            var account = _context.Accounts.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (account == null) throw new AppException("Invalid token");
            return account;
        }

        private Account getAccountByResetToken(string token)
        {
            var account = _context.Accounts.SingleOrDefault(x =>
                x.ResetToken == token && x.ResetTokenExpires > DateTime.UtcNow);
            if (account == null) throw new AppException("Invalid token");
            return account;
        }

        private string generateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string generateResetToken()
        {
           
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder(6);

            for (int i = 0; i < 6; i++)
            {
                int digit = random.Next(10); // Generates a random number between 0 and 9
                stringBuilder.Append(digit);
            }

            return stringBuilder.ToString();
        }

        private string generateVerificationToken()
        {
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            //  var token = new Random().Next(0,999999).ToString();

            // ensure token is unique by checking against db
            var tokenIsUnique = !_context.Accounts.Any(x => x.VerificationToken == token);
            if (!tokenIsUnique)
                return generateVerificationToken();

            return token;
        }

        private RefreshToken rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void removeOldRefreshTokens(Account account)
        {
            account.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private void revokeDescendantRefreshTokens(RefreshToken refreshToken, Account account, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = account.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                    revokeRefreshToken(childToken, ipAddress, reason);
                else
                    revokeDescendantRefreshTokens(childToken, account, ipAddress, reason);
            }
        }

        private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }
        private void sendPasswordResetEmail(Account account, string resetUrl)
        {
            string message;


            message = $@"<p>Please click the below link to reset your password, the link will be valid for 10 minutes:</p>
                            <p><a href=""{resetUrl}"">Reset Password</a></p>";

            _emailService.SendEmailWithMailGun(new MailGunMessage
            {
                To = new List<string> { account.Email },
                Subject = "INTEL - Reset Password",
                HtmlContent = $@"<h4>Reset Password Email</h4>
                        {message}"
            });
        }

        private void sendTokenVerificationEmail(Account account, string token)
        {
            string message;


            message = $@"<p>Please enter the token below to verify your account, the token will be valid for 10 minutes:</p>
                        <p><b>{token}</b></p>";

            _emailService.SendEmailWithMailGun(new MailGunMessage
            {
                To = new List<string> { account.Email },
                Subject = "INTEL - Verification Token",
                HtmlContent = $@"<h4>Verification Token Email</h4>
                        {message}"
            });
        }

        #endregion


    }
}
