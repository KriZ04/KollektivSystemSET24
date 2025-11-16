using System;
using System.Linq;
using System.Security.Claims;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Enums;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KollektivSystem.ApiService.Extensions.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users");
        // Opprett ny bruker (lar det stå, men låser til Staff)
        //app.MapPost("/users", async (User dto, IUserRepository users, IUnitOfWork uow, CancellationToken ct) =>
        //{
        //    var user = new User
        //    {
        //        DisplayName = dto.DisplayName,
        //        Role = dto.Role,
        //        Provider = dto.Provider,
        //        Sub = dto.Sub,
        //        CreatedAt = dto.CreatedAt,
        //        UpdatedAt = dto.UpdatedAt,
        //        LastLogin = dto.LastLogin,
        //        Email = dto.Email,
        //    };

        //    await users.AddAsync(user, ct);
        //    await uow.SaveChangesAsync(ct);

        //    return Results.Created($"/users/{user.Id}", user);
        //})
        //.RequireAuthorization("Staff");

        // Hent info om innlogget bruker
        group.MapGet("/me", HandleMe).RequireAuthorization();

        // Liste ALLE brukere – for admin/sysutvikler-UI
        group.MapGet("", HandleGetAll).RequireAuthorization("Staff");

        // Endre rollen til en bruker (kun SystemManager / "Manager"-policy)
        group.MapPut("{id:guid}/role", HandleRoleUpdate).RequireAuthorization("Manager"); // SystemManager

        return app;
    }

    internal static async Task<IResult> HandleMe(ClaimsPrincipal user, IUserService userService, CancellationToken ct)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(id))
            return Results.Unauthorized();

        if (!Guid.TryParse(id, out var guid))
            return Results.Unauthorized();

        var u = await userService.GetMeByIdAsync(guid, ct);

        if (u is null)
            return Results.NotFound();

        return Results.Ok(u);
    }
    internal static async Task<IResult> HandleGetAll(IUserService userService, CancellationToken ct)
    {
        var users = await userService.GetAllAsync(ct);
        return Results.Ok(users);
    }
    internal static async Task<IResult> HandleRoleUpdate(Guid id, UpdateUserRoleRequest request, IUserService userService, CancellationToken ct)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Role))
        {
            return Results.BadRequest("Role is required.");
        }

        if (!Enum.TryParse<Role>(request.Role, ignoreCase: true, out var newRole))
        {
            return Results.BadRequest($"Unknown role: {request.Role}");
        }

        var updated = await userService.UpdateRoleAsync(id, newRole, ct);
        if (!updated)
            return Results.NotFound();

        return Results.NoContent();
    }

}

// Body til PUT /users/{id}/role → { "role": "Admin" }
public sealed record UpdateUserRoleRequest(string Role);
