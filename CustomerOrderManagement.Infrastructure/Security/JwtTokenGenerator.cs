using CustomerOrderManagement.Application.Authentication;
using CustomerOrderManagement.Application.DTOs.Authentication;
using CustomerOrderManagement.Application.Interfaces.Security;
using CustomerOrderManagement.Domain.Entities;
using CustomerOrderManagement.Infrastructure.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerOrderManagement.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _settings;

        public JwtTokenGenerator(JwtSettings settings)
        {
            _settings = settings;
        }

        public LoginResponseDto GenerateToken(ApplicationUser user,string role)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(
                _settings.ExpirationMinutes);

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id),

                new Claim(
                    ClaimTypes.Name,
                    user.UserName),

                new Claim(
                    ClaimTypes.Email,
                    user.Email),

                new Claim(
                    ClaimTypes.Role,
                    role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.Secret));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            var tokenString =
                new JwtSecurityTokenHandler()
                    .WriteToken(token);

            return new LoginResponseDto
            {
                Token = tokenString,
                ExpiresAt = expiresAt,
                UserId = user.Id,
                Email = user.Email,
                Role = role
            };
        }
    }
}
