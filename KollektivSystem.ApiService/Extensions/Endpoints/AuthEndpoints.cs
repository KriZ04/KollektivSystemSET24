using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace KollektivSystem.ApiService.Extensions.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/auth");

            group.MapGet("/login", (HttpRequest req, IAuthProvider authProvider, IMemoryCache cache) =>
            {
                var callback = new Uri($"{req.Scheme}://{req.Host}/auth/callback");
                var ch = authProvider.BuildAuthorizeRedirect(callback, ["openid", "email", "profile"]);
                cache.Set($"oidc_state:{ch.State}", true, TimeSpan.FromMinutes(5));
            });


            return app;
        }
    }
}
