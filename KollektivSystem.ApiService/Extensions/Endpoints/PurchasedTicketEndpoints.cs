using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.Tickets;
using KollektivSystem.ApiService.Models.Mappers;
using KollektivSystem.ApiService.Services.Interfaces;
using System.Security.Claims;

namespace KollektivSystem.ApiService.Extensions.Endpoints;

public static class PurchasedTicketEndpoints
{
    public static IEndpointRouteBuilder MapPurchasedTicketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tickets").RequireAuthorization();

        group.MapPost("", HandleBuyTicket);
        group.MapPost("validate", HandleTicketValidation).RequireAuthorization("Staff");

        var me = group.MapGroup("me");
        me.MapGet("", HandleGetMe);
        me.MapGet("{id:guid}", HandleGetMeById);

        var admin = group.MapGroup("admin").RequireAuthorization("Staff");
        admin.MapPost("{id:guid}/revoke", HandleRevoke);
        admin.MapGet("{id:guid}", HandleGetById);

        return app;
    }
    internal static async Task<IResult> HandleBuyTicket(PurchaseTicketRequest req, IPurchasedTicketService ticketService, ClaimsPrincipal user, CancellationToken ct)
    {
        var userId = user.GetUserId();

        var purchasedTicket = await ticketService.PurchaseAsync(userId, req.TicketTypeId, ct);
        var dto = purchasedTicket.ToResponse();
        return Results.Created($"me/{dto.Id}", dto);
    }
    internal static async Task<IResult> HandleTicketValidation(ValidateTicketRequest req, IPurchasedTicketService ticketService, CancellationToken ct)
    {
        var (isValid, ticket, reason) = await ticketService.ValidateAsync(req.ValidationCode, ct);

        if (!isValid || ticket is null)
        {
            return Results.Ok(new ValidateTicketResponse
            {
                IsValid = false,
                Reason = reason ?? "Invalid or expired ticket"
            });
        }

        return Results.Ok(new ValidateTicketResponse
        {
            IsValid = true,
            TicketId = ticket.Id,
            ExpireAt = ticket.ExpireAt
        });
    }
    internal static async Task<IResult> HandleGetMe(bool includeExpired, IPurchasedTicketService ticketService, ClaimsPrincipal user, CancellationToken ct)
    {
        var userId = user.GetUserId();
        var tickets = await ticketService.GetByUserAsync(userId, includeExpired, ct);
        var dtos = tickets.Select(t => t.ToResponse());
        return Results.Ok(dtos);
    }
    internal static async Task<IResult> HandleGetMeById(Guid id, IPurchasedTicketService ticketService, ClaimsPrincipal user, CancellationToken ct)
    {
        var userId = user.GetUserId();

        var ticket = await ticketService.GetForUserByIdAsync(userId, id, ct);

        if (ticket is null)
            return Results.NotFound();

        return Results.Ok(ticket.ToResponse());


    }
    internal static async Task<IResult> HandleRevoke(Guid id, IPurchasedTicketService ticketService, CancellationToken ct)
    {
        var isSuccess = await ticketService.RevokeAsync(id, ct);
        if (!isSuccess)
            return Results.NotFound();
        return Results.NoContent();
    }
    internal static async Task<IResult> HandleGetById(Guid id, IPurchasedTicketService ticketService, CancellationToken ct)
    {
        var ticket = await ticketService.GetByIdAsync(id, ct);
        if (ticket == null)
            return Results.NotFound();
        return Results.Ok(ticket.ToResponse());
    }
}
