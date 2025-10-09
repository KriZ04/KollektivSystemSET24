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
                    DisplayName = dto.DisplayName, 
                    Role = dto.Role, 
                    Provider = dto.Provider,
                    Sub = dto.Sub,
                    CreatedAt = dto.CreatedAt,
                    UpdatedAt = dto.UpdatedAt,
                    LastLogin = dto.LastLogin,
                    Email = dto.Email,
                };

                await users.AddAsync(user, ct);
                await uow.SaveChangesAsync(ct);

                return Results.Created($"/users/{user.Id}", user);
            });

            return routes;
        }
    }
}
