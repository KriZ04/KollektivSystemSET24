
using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services.Interfaces;

namespace KollektivSystem.ApiService.Services
{
    public sealed class TokenService : ITokenService
    {
        private readonly IJwtIssuer _issuer;
        private readonly IUnitOfWork _uow;

        public TokenService(IJwtIssuer issuer, IUnitOfWork uow)
        {
            _issuer = issuer;
            _uow = uow;
        }


        public async Task<(bool Success, string? AccessToken, string? RefreshToken)> RefreshAsync(string refreshToken, CancellationToken ct = default)
        {
            var incomingHash = TokenHash.Hash(refreshToken);


            var token = await _uow.RefreshTokens.GetByHashAsync(incomingHash, ct);
            if (token is null || token.Revoked || token.ExpiresAt < DateTime.UtcNow)
                return (false, null, null);

            var user = await _uow.Users.FindAsync(token.UserId, ct);
            if (user is null)
                return (false, null, null);

            token.Revoked = true;

            var (newRefreshPlain, newRefresh) = RefreshTokenFactory.CreateForUser(user);
            await _uow.RefreshTokens.AddAsync(newRefresh, ct);

            var newAccess = _issuer.Issue(new(user.Id, user.Role.ToString(), "Refresh", user.Sub));

            await _uow.SaveChangesAsync(ct);

            return (true, newAccess, newRefreshPlain);
        }
    }
}
