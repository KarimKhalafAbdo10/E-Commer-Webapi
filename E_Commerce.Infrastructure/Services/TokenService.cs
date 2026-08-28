using E_Commerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Services
{
    internal class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }
        public string GetToken(string userId, string Email, string UserName, IReadOnlyList<string> roles)
        {
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.Name, UserName)

            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));


            var secKey = _jwtSettings.SecretKey;

            if (string.IsNullOrEmpty(secKey))
            {
                throw new InvalidOperationException("JWT Secret Key is not configured.");
            }

            if (secKey.Length < 32)
            {
                throw new InvalidOperationException("JWT Secret Key must be at least 32 characters long.");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey));
            var cardentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                    signingCredentials: cardentials



                );


            return new JwtSecurityTokenHandler().WriteToken(token);


        }



       
    }
    public class JwtSettings
        {
            public string Issuer { get; set; } = default!;
            public string Audience { get; set; } = default!;
            public string SecretKey { get; set; } = default!;
            public int ExpirationMinutes { get; set; }

        }
}
