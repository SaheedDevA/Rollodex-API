using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Request
{
    public class AuthenticateRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    
    }

    public class CreateWorkSpaceRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class VerifyTokendRequest
    {
        [Required]
        public string Token { get; set; }

    }

    public class SetupWorkSpaceRequest
    {
        [Required]
        [EmailAddress]
        public string AdminEmail { get; set; }
        [Required]
        public string WorkSpaceName { get; set; } //get worspace url from name
        [Required]
        public string Color { get; set; }
        [Required]
        public string base64StringLogo { get;set; }
    }

    public class ResetPasswordConfirmed
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }


    public class ResetPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string ResetPasswordUrl { get; set; }   
}


    public class ChangePasswordRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }

}
