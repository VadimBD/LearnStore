using LearnStore.Application.DTO;
using LearnStore.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LearnStore.Infrastructure.Auth
{
    public class JwtTokenGenerator(IJwtOptionsProvider optionsProvider) : IJwtTokenGenerator
    {
        public string GenerateToken(UserDto user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.UniqueName, user.Name),
                new ("roles", string.Join(",", user.Roles))
            };
            var options = optionsProvider.GetOptions();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(options.ExpireHours),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
