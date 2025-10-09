using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Enums;
using System.Security.Claims;

namespace KollektivSystem.ApiService.Services
{
    public class AuthService : IAuthService
    {
        public Task<(User user, string apiJwt)> SignInWithIdTokenAsync(AuthProvider provider, ClaimsPrincipal principal, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
