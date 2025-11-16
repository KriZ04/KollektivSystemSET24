using System.Security.Claims;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Enums;
using KollektivSystem.ApiService.Models.Mappers;
using KollektivSystem.ApiService.Services.Interfaces;

namespace KollektivSystem.ApiService.Extensions.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users");

        group.MapGet("me", HandleMe).RequireAuthorization();

        group.MapGet("", HandleGetAll).RequireAuthorization("Staff");

        group.MapPut("{id:guid}/role", HandleRoleUpdate).RequireAuthorization("Manager");

        group.MapGet("{id:guid}/tickets", HandleGetUserTickets).RequireAuthorization("Staff");


        return app;
    }

    internal static async Task<IResult> HandleMe(ClaimsPrincipal user, IUserService userService, CancellationToken ct)
    {
        var userId = user.GetUserId();

        var u = await userService.GetMeByIdAsync(userId, ct);

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
    internal static async Task<IResult> HandleGetUserTickets(Guid id, IPurchasedTicketService purchaseService, CancellationToken ct)
    {
        var tickets = await purchaseService.GetByUserIdAsync(id, ct);
        return Results.Ok(tickets.Select(t => t.ToResponse()));
    }

}

// Body til PUT /users/{id}/role → { "role": "Admin" }
public sealed record UpdateUserRoleRequest(string Role);
