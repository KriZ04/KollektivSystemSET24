using KollektivSystem.ApiService.Extensions.Auth;

namespace KollektivSystem.ApiService.Extensions.ServiceExtensions
{
    public static class EnsureUserExistsExtensions
    {
        public static IApplicationBuilder UseEnsureUserExists(this IApplicationBuilder app)
            => app.UseMiddleware<EnsureUserExists>();
    }
}
