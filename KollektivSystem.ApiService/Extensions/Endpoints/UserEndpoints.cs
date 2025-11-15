using System;
using System.Linq;
using System.Security.Claims;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Enums;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;
using Microsoft.AspNetCore.Mvc;

namespace KollektivSystem.ApiService.Extensions.Endpoints
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            // Opprett ny bruker (lar det stå, men låser til Staff)
            app.MapPost("/users", async (User dto, IUserRepository users, IUnitOfWork uow, CancellationToken ct) =>
            {
                var user = new User
                {
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
            })
            .RequireAuthorization("Staff");

            // Hent info om innlogget bruker
            app.MapGet("/users/me", async (ClaimsPrincipal user, IUserRepository repo, CancellationToken ct) =>
            {
                var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(id))
                    return Results.Unauthorized();

                if (!Guid.TryParse(id, out var guid))
                    return Results.Unauthorized();

                var u = await repo.FindAsync(guid, ct);
                if (u is null)
                    return Results.NotFound();

                return Results.Ok(new
                {
                    id = u.Id,
                    name = u.DisplayName,
                    email = u.Email
                });
            })
            .RequireAuthorization();

            // Liste ALLE brukere – for admin/sysutvikler-UI
            app.MapGet("/users", async (IUserRepository users, CancellationToken ct) =>
            {
                var list = await users.GetAllAsync(ct);

                var dto = list.Select(u => new
                {
                    id = u.Id,
                    name = u.DisplayName,
                    email = u.Email,
                    role = u.Role.ToString(),
                    provider = u.Provider.ToString(),
                    sub = u.Sub,
                    lastLogin = u.LastLogin
                });

                return Results.Ok(dto);
            })
            // Bare Staff (Admin/Developer/SystemManager) kan se alle brukere
            .RequireAuthorization("Staff");

            // Endre rollen til en bruker (kun SystemManager / "Manager"-policy)
            app.MapPut("/users/{id:guid}/role", async (
                Guid id,
                [FromBody] UpdateUserRoleRequest request,
                IUserRepository users,
                IUnitOfWork uow,
                CancellationToken ct) =>
            {
                if (request is null || string.IsNullOrWhiteSpace(request.Role))
                {
                    return Results.BadRequest("Role is required.");
                }

                if (!Enum.TryParse<Role>(request.Role, ignoreCase: true, out var newRole))
                {
                    return Results.BadRequest($"Unknown role: {request.Role}");
                }

                var user = await users.FindAsync(id, ct);
                if (user is null)
                    return Results.NotFound();

                user.Role = newRole;
                user.UpdatedAt = DateTime.UtcNow;

                users.Update(user);
                await uow.SaveChangesAsync(ct);

                return Results.NoContent();
            })
            .RequireAuthorization("Manager"); // SystemManager

            return app;
        }
    }

    // Body til PUT /users/{id}/role → { "role": "Admin" }
    public sealed record UpdateUserRoleRequest(string Role);
}
