using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KollektivSystem.ApiService.Infrastructure
{
    public class JwtIssuer : IJwtIssuer
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _key;

        public JwtIssuer(string issuer, string audience, string key)
        {
            _issuer = issuer;
            _audience = audience;
            _key = key;
        }

        public string Issue(ApiClaims apiClaims)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, apiClaims.UserId.ToString()),
                new(ClaimTypes.Role, apiClaims.Role),
                new("provider", apiClaims.Provider),
                new("sub", apiClaims.Sub)
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: now,
                expires: now.AddHours(2),
                signingCredentials: creds);

            return handler.WriteToken(token);
        }
    }
}
