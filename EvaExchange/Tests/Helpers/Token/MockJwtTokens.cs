using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Tests.Helpers.Token
{
    public static class MockJwtTokens
    {
        private static readonly JwtSecurityTokenHandler STokenHandler = new();
        private static string _keyString = "Ec8NB%@aaB<YGpQ9v-3dsB_hp'ZFL9x==S^(";

        static MockJwtTokens()
        {
            var SKey = Encoding.UTF8.GetBytes(_keyString);
            SecurityKey = new SymmetricSecurityKey(SKey);
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
        }

        public static string Issuer { get; } = "eva.guru";
        public static string Audience { get; } = "eva.guru";
        public static SecurityKey SecurityKey { get; }
        public static SigningCredentials SigningCredentials { get; }

        public static string GenerateJwtToken(IEnumerable<Claim> claims, double value = 5)
        {
            return STokenHandler.WriteToken(new JwtSecurityToken(Issuer, Audience, claims, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(value), SigningCredentials));
        }
    }
}