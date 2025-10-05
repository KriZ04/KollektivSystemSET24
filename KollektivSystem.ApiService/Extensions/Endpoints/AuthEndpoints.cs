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
                    .FirstOrDefaultAsync(u => u.email == request.email, ct);

                if (user == null)
                    return Results.Unauthorized();

                // For now, assume plain-text passwords (you can add hashing later)
                if (user.password != request.password)
                    return Results.Unauthorized();

                // Optionally update last_login timestamp
                user.last_login = DateTime.UtcNow;

                // Persist the update through the Unit of Work
                await uow.SaveChangesAsync(ct);

                // Return basic user info
                return Results.Ok(new
                {
                    message = "Login successful",
                    user = new
                    {
                        user.Id,
                        user.display_name,
                        user.email,
                        user.Role,
                        user.last_login
                    }
                });
            })
            .WithName("Login");

            return routes;
        }

        public record LoginRequest(string email, string password);
    }
}
