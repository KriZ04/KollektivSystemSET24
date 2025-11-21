using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Enums;
using System.Security.Claims;

namespace KollektivSystem.ApiService.Services.Interfaces;

public interface IAuthService
{
    Task<(User user, string apiJwt, string refreshToken)> SignInWithIdTokenAsync(AuthProvider provider, ClaimsPrincipal principal, CancellationToken ct);
}