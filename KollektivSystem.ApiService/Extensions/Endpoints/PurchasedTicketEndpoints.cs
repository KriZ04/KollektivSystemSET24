using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.Tickets;
using KollektivSystem.ApiService.Models.Mappers;
using KollektivSystem.ApiService.Services.Interfaces;
using System.Security.Claims;

namespace KollektivSystem.ApiService.Extensions.Endpoints;

public sealed class PurchasedTicketEndpointsLogggerCategory { }
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
    internal static async Task<IResult> HandleBuyTicket(PurchaseTicketRequest req, IPurchasedTicketService ticketService, ClaimsPrincipal user, ILogger<PurchasedTicketEndpointsLogggerCategory> logger, CancellationToken ct)
    {
        var userId = user.GetUserId();
        logger.BuyTicketRequested(userId, req.TicketTypeId);

        var purchasedTicket = await ticketService.PurchaseAsync(userId, req.TicketTypeId, ct);
        if (purchasedTicket == null)
        {
            logger.BuyTicketFailed(userId, req.TicketTypeId);
            return Results.Problem();
        }
        logger.BuyTicketSucceeded(purchasedTicket.Id, userId, req.TicketTypeId);

        var dto = purchasedTicket.ToResponse();
        return Results.Created($"me/{dto.Id}", dto);
    }
    internal static async Task<IResult> HandleTicketValidation(ValidateTicketRequest req, IPurchasedTicketService ticketService, ILogger<PurchasedTicketEndpointsLogggerCategory> logger, CancellationToken ct)
    {
        logger.TicketValidationRequested(req.ValidationCode);
        var (isValid, ticket, reason) = await ticketService.ValidateAsync(req.ValidationCode, ct);

        if (!isValid || ticket is null)
        {
            var msg = reason ?? "Invalid or expired ticket";
            logger.TicketValidationFailed(req.ValidationCode, msg);
            return Results.Ok(new ValidateTicketResponse
            {
                IsValid = false,
                Reason = msg
            });
        }
        logger.TicketValidationSucceeded(ticket.Id, req.ValidationCode);
        return Results.Ok(new ValidateTicketResponse
        {
            IsValid = true,
            TicketId = ticket.Id,
            ExpireAt = ticket.ExpireAt
        });
    }
    internal static async Task<IResult> HandleGetMe(bool includeInvalid, IPurchasedTicketService ticketService, ClaimsPrincipal user, ILogger<PurchasedTicketEndpointsLogggerCategory> logger, CancellationToken ct)
    {
        var userId = user.GetUserId();
        logger.GetMyTicketsRequested(userId, includeInvalid);

        var tickets = await ticketService.GetByUserAsync(userId, includeInvalid, ct);
        var dtos = tickets.Select(t => t.ToResponse());

        return Results.Ok(dtos);
    }
    internal static async Task<IResult> HandleGetMeById(Guid id, IPurchasedTicketService ticketService, ClaimsPrincipal user, ILogger<PurchasedTicketEndpointsLogggerCategory> logger, CancellationToken ct)
    {
        var userId = user.GetUserId();

        var ticket = await ticketService.GetForUserByIdAsync(userId, id, ct);

        if (ticket is null)
        {
            logger.GetMyTicketByIdNotFound(userId, id);
            return Results.NotFound();
        }

        logger.GetMyTicketByIdSucceeded(userId, id);
        return Results.Ok(ticket.ToResponse());


    }
    internal static async Task<IResult> HandleRevoke(Guid id, IPurchasedTicketService ticketService, ILogger<PurchasedTicketEndpointsLogggerCategory> logger, CancellationToken ct)
    {
        logger.RevokeTicketRequested(id);
        var isSuccess = await ticketService.RevokeAsync(id, ct);
        if (!isSuccess)
        {
            logger.RevokeTicketNotFound(id);
            return Results.NotFound();
        }

        logger.RevokeTicketSucceeded(id);
        return Results.NoContent();
    }
    internal static async Task<IResult> HandleGetById(Guid id, IPurchasedTicketService ticketService, ILogger<PurchasedTicketEndpointsLogggerCategory> logger, CancellationToken ct)
    {
        logger.GetTicketByIdRequested(id);
        var ticket = await ticketService.GetByIdAsync(id, ct);
        if (ticket == null)
        {
            logger.GetTicketByIdNotFound(id);
            return Results.NotFound();
        }

        logger.GetTicketByIdSucceeded(id);
        return Results.Ok(ticket.ToResponse());
    }
}
