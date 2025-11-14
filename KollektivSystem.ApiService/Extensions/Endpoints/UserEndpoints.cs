using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;
using System.Security.Claims;

namespace KollektivSystem.ApiService.Extensions.Endpoints
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
        {

            builder.MapPost("/users", async (User dto, IUserRepository users, IUnitOfWork uow, CancellationToken ct) =>
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
            }).RequireAuthorization("Staff");

            builder.MapGet("/users/me", async (ClaimsPrincipal user, IUserRepository repo, CancellationToken ct) =>
            {
                var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(id)) return Results.Unauthorized();

                var guid = Guid.Parse(id);
                var u = await repo.FindAsync(guid, ct);
                if (u is null) return Results.NotFound();

                return Results.Ok(new
                {
                    id = u.Id,
                    name = u.DisplayName,
                    email = u.Email,
                    role = u.Role
                });
            })
            .RequireAuthorization(); 


            return builder;
        }
    }
}
