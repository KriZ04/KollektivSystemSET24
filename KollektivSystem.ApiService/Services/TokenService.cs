
using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;

namespace KollektivSystem.ApiService.Services
{
    public sealed class TokenService : ITokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IUserRepository _userRepo;
        private readonly IJwtIssuer _issuer;
        private readonly IUnitOfWork _uow;

        public TokenService(IRefreshTokenRepository refreshTokenRepo, IUserRepository userRepo, IJwtIssuer issuer, IUnitOfWork uow)
        {
            _refreshTokenRepo = refreshTokenRepo;
            _userRepo = userRepo;
            _issuer = issuer;
            _uow = uow;
        }


        public async Task<(bool Success, string? AccessToken, string? RefreshToken)> RefreshAsync(string refreshToken, CancellationToken ct = default)
        {
            var incomingHash = TokenHash.Hash(refreshToken);


            var token = await _refreshTokenRepo.GetByHashAsync(incomingHash, ct);
            if (token is null || token.Revoked || token.ExpiresAt < DateTime.UtcNow)
                return (false, null, null);

            var user = await _userRepo.FindAsync(token.UserId, ct);
            if (user is null)
                return (false, null, null);

            token.Revoked = true;

            var (newRefreshPlain, newRefresh) = RefreshTokenFactory.CreateForUser(user);
            await _refreshTokenRepo.AddAsync(newRefresh, ct);

            var newAccess = _issuer.Issue(new(user.Id, user.Role.ToString(), "Refresh", user.Sub));

            await _uow.SaveChangesAsync(ct);

            return (true, newAccess, newRefreshPlain);
        }
    }
}
