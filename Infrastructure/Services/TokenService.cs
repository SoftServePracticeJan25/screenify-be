using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            var signingKey = _config["JWT:SigningKey"] ?? throw new ArgumentNullException("JWT:SigningKey is not configured.");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }
        public string CreateAccessToken(AppUser user, List<string> roles)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null when generating a token.");
            }

            var email = user.Email ?? throw new ArgumentNullException(nameof(user.Email), "User email cannot be null.");
            var userName = user.UserName ?? throw new ArgumentNullException(nameof(user.UserName), "User name cannot be null.");

            if (roles == null || !roles.Any())
                throw new ArgumentException("User must have at least one role.", nameof(roles));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.GivenName, userName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id) // Added userId in JWT TOKEN
            };
            
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); // Added Role in JWT TOKEN
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
