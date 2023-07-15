using Rollodex.lib.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Response
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(Account account, string baseUrl, string jwt, string refrshTwkn)
        {
            Id = account.Id;
            FirstName = account.FirstName;
            LastName = account.LastName;
            Email = account.Email;
            Username = account.Username;
            PhoneNumber = account.PhoneNumber;
            UserType = account.UserType;
            ImageUrl = baseUrl + account.Image;
            JwtToken = jwt;
            RefreshToken = refrshTwkn;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string UserType { get; set; }
        public string? ImageUrl { get; set; }
        public string JwtToken { get; set; }
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
    }
}
