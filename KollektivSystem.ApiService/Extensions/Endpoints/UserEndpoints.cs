using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;

namespace KollektivSystem.ApiService.Extensions.Endpoints
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/users", async (User dto, IUserRepository users, IUnitOfWork uow, CancellationToken ct) =>
            {
                var user = new User { 
                    display_name = dto.display_name, 
                    Role = dto.Role, 
                    provider = dto.provider,
                    sub = dto.sub,
                    created_at = dto.created_at,
                    updated_at = dto.updated_at,
                    last_login = dto.last_login,
                    email = dto.email,
                };

                await users.AddAsync(user, ct);
                await uow.SaveChangesAsync(ct);

                return Results.Created($"/users/{user.Id}", user);
            });

            return routes;
        }
    }
}
