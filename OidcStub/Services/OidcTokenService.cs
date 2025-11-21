using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OidcStub.Exceptions;
using OidcStub.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OidcStub.Services
{
    public sealed class OidcTokenService: IOidcTokenService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<OidcTokenService> _logger;
        private readonly OidcOptions _cfg;

        public OidcTokenService(IMemoryCache cache, IOptionsMonitor<OidcOptions> cfg, ILogger<OidcTokenService> logger)
        {
            _cache = cache;
            _logger = logger;
            _cfg = cfg.CurrentValue;
        }

        public TokenResponse ExchangeCode(IFormCollection form, CancellationToken ct)
        {
            var grantType = form["grant_type"].ToString();
            var code = form["code"].ToString();
            var clientId = form["client_id"].ToString();
            var clientSecret = form["client_secret"].ToString();
            //var redirectUri = form["redirect_uri"].ToString();

            if (!string.Equals(grantType, "authorization_code", StringComparison.Ordinal))
                throw new OidcException("unsupported_grant_type");

            if (!string.Equals(clientId, _cfg.ClientId, StringComparison.Ordinal) ||
                !string.Equals(clientSecret, _cfg.ClientSecret, StringComparison.Ordinal))
                throw new OidcException("invalid_client");

            if (string.IsNullOrWhiteSpace(code))
                throw new OidcException("invalid_grant");


            if (!_cache.TryGetValue($"auth_code:{code}", out Identity? ident) || ident is null)
                throw new OidcException("invalid_grant");

            _cache.Remove($"auth_code:{code}");

            // OLD Debugging logs
            //_logger.LogInformation("OIDC SigningKey first8='{KeyStart}', lenChars={LenChars}", 
            //    _cfg.SigningKey?.Substring(0, Math.Min(8, _cfg.SigningKey?.Length ?? 0)), 
            //    _cfg.SigningKey?.Length ?? 0);

            //var keyBytes = Encoding.UTF8.GetBytes(_cfg.SigningKey ?? "");
            //_logger.LogInformation("OIDC keyBytes length={LenBytes}", keyBytes.Length);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg.SigningKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new("sub",  ident.Sub),
                new("name", ident.Name),
            };
            
            if (!string.IsNullOrWhiteSpace(ident.Email))
                claims.Add(new("email", ident.Email!));

            var idJwt = new JwtSecurityToken(
                issuer: _cfg.Issuer,
                audience: _cfg.ClientId,
                claims: claims,
                notBefore: now,
                expires: now.AddHours(1),
                signingCredentials: creds);

            var idToken = new JwtSecurityTokenHandler().WriteToken(idJwt);

            return new TokenResponse(
                IdToken: idToken,
                AccessToken: Guid.NewGuid().ToString("N"),
                TokenType: "Bearer",
                ExpiresIn: 3600);
        }
    }
}
