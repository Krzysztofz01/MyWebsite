using PersonalWebsiteWebApi.Models;
using PersonalWebsiteWebApi.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalWebsiteWebApi.Services
{
    public interface IAuthenticationService
    {
        public string GenerateToken(User user);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly string Role = "Admin";
        private readonly JsonWebTokenSettings jsonWebTokenSettings;

        public AuthenticationService(
            IOptions<JsonWebTokenSettings> jsonWebTokenSettings)
        {
            this.jsonWebTokenSettings = jsonWebTokenSettings.Value;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, Role)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jsonWebTokenSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
