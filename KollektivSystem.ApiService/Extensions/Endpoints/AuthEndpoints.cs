using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;
using Microsoft.EntityFrameworkCore;

namespace KollektivSystem.ApiService.Extensions.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/auth/login", async (
                LoginRequest request,
                IUserRepository users,
                IUnitOfWork uow,
                CancellationToken ct) =>
            {
                // Find the user by email
                var user = await users.Query()
                    .FirstOrDefaultAsync(u => u.Email == request.email, ct);

                if (user == null)
                    return Results.Unauthorized();

                // For now, assume plain-text passwords (you can add hashing later)
                if (user.password != request.password)
                    return Results.Unauthorized();

                // Optionally update last_login timestamp
                user.LastLogin = DateTime.UtcNow;

                // Persist the update through the Unit of Work
                await uow.SaveChangesAsync(ct);

                // Return basic user info
                return Results.Ok(new
                {
                    message = "Login successful",
                    user = new
                    {
                        user.Id,
                        user.DisplayName,
                        user.Email,
                        user.Role,
                        user.LastLogin
                    }
                });
            })
            .WithName("Login");

            return routes;
        }

        public record LoginRequest(string email, string password);
    }
}
