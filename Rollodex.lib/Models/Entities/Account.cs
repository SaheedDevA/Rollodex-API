using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class Account : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string VerificationToken { get; set; }
        public DateTime? Verified { get; set; }
        public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime LastTimeLoggedIn { get; set; }
        public DateTime LoggedOutTime { get; set; }
        public string UserType { get; set; }
        public string? Image { get; set; }

        public bool OwnsToken(string token)
        {
            return RefreshTokens?.Find(x => x.Token == token) != null;
        }

        [NotMapped]
        public string getFulllName => $"{FirstName} {LastName}";

        //sec keys
        public int SystemId { get;set; }
        
    }
}
