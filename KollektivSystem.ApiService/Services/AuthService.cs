using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Enums;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace KollektivSystem.ApiService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtIssuer _issuer;
        private readonly IUserRepository _userRepo;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IUnitOfWork _uow;

        public AuthService(IJwtIssuer issuer, IUserRepository userRepo, IRefreshTokenRepository refreshTokenRepo, IUnitOfWork uow)
        {
            _issuer = issuer;
            _userRepo = userRepo;
            _refreshTokenRepo = refreshTokenRepo;
            _uow = uow;
        }

        public async Task<(User user, string apiJwt, string refreshToken)> SignInWithIdTokenAsync(
            AuthProvider provider,
            ClaimsPrincipal id,
            CancellationToken ct)
        {
            var sub = id.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("missing sub");
            var email = id.FindFirst(ClaimTypes.Email)?.Value;
            var name = id.FindFirst(ClaimTypes.Name)?.Value ?? email ?? "user";

            var user = await _userRepo.GetByProviderSubAsync(provider, sub, ct);

            if (user is null)
            {
                
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Provider = provider,
                    Sub = sub,
                    DisplayName = name,
                    Email = email,
                    Role = Role.Customer
                };

                if (sub == "sub-olav")
                {
                    user.Role = Role.SystemManager;
                }

                await _userRepo.AddAsync(user);
            }
            else
            {
                user.DisplayName = name;
                user.Email = email ?? user.Email;
                user.UpdateLogin();
                //user.UpdatedAt = DateTime.UtcNow;

                _userRepo.Update(user);
            }

            await _uow.SaveChangesAsync(ct);

            var apiJwt = _issuer.Issue(new(user.Id, user.Role.ToString(), provider.ToString(), sub));

            var (refreshPlain, refreshEntity) = RefreshTokenFactory.CreateForUser(user);

            await _refreshTokenRepo.AddAsync(refreshEntity, ct);
            await _uow.SaveChangesAsync(ct);

            return (user, apiJwt, refreshPlain);
        }
    }
}
