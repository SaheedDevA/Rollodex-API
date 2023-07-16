using Intel.Lib.Services;
using IntelApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Rollodex.lib.Models.Request;
using Rolodex.Lib.Utils.Authorization;

namespace Rollodex.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountServices _accountServices;
        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(AuthenticateRequest model)
        {
            var response = _accountServices.Authenticate(model, ipAddress());
            setTokenCookie(response.Data.RefreshToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("Accounts")]
        public IActionResult GetAccounts()
        {
            var response = _accountServices.GetAccounts();
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpPost("CreateWorkSpace")]
        public IActionResult CreateWorkSpace(CreateWorkSpaceRequest model)
        {
            var response = _accountServices.CreateWorkSpace(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("SetUpWorkSpace")]
        public IActionResult SetUpWorkSpace(SetupWorkSpaceRequest model)
        {
            var response = _accountServices.SetUpWorkSpace(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("GetWorksSpaces")]
        public IActionResult GetWorksSpaces()
        {
            var response = _accountServices.GetWorkSpaces();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("VerifyToken")]
        public IActionResult VerifyToken(VerifyTokendRequest model)
        {
            var response = _accountServices.VerifyToken(model);
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpPost("ResetPasswordRequest")]
        public IActionResult ResetPasswordRequest(ResetPasswordRequest model)
        {
            var response = _accountServices.ResetPasswordRequest(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("ResetPasswordConfirmed")]
        public IActionResult ResetPasswordConfirmed(ResetPasswordConfirmed model)
        {
            var response = _accountServices.ResetPasswordConfirmed(model);
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordRequest model)
        {
            var response = _accountServices.ChangePassword(model);
            return Ok(response);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

    }
}
