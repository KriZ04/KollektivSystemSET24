using KollektivSystem.ApiService.Infrastructure.Logging.Endpoints;
using KollektivSystem.ApiService.Models.Dtos.TicketTypes;
using KollektivSystem.ApiService.Models.Mappers;
using KollektivSystem.ApiService.Services.Interfaces;

namespace KollektivSystem.ApiService.Extensions.Endpoints;
public sealed class TicketTypeEndpointsLoggerCategory { }
public static class TicketTypeEndpoints
{
    public static IEndpointRouteBuilder MapTicketTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tickets-type");

        group.MapGet("", HandleGetAllActive);
        group.MapGet("{id:int}", HandleGetActiveTicketById);

        var admin = group.MapGroup("admin").RequireAuthorization("Staff");
        admin.MapGet("", HandleGetAll);
        admin.MapGet("{id:int}", HandleGetTicketById);
        admin.MapPost("", HandleCreateNew);
        admin.MapDelete("{id:int}", HandleDelete);
        admin.MapPost("{id:int}/activate", HandleActivate);

        return app;

    }

    internal static async Task<IResult> HandleGetAll(ITicketTypeService ttService, ILogger<TicketTypeEndpointsLoggerCategory> logger, CancellationToken ct)
    {
        const bool activeOnly = false;
        logger.ListTicketTypesRequested(activeOnly);

        var ticketTypes = await ttService.GetAllAsync(ct);
        if (ticketTypes == null || !ticketTypes.Any())
        {
            logger.ListTicketTypesNotFound(activeOnly);
            return Results.NotFound();
        }


        logger.ListTicketTypesSucceeded(ticketTypes.Count, activeOnly);

        return Results.Ok(ticketTypes.Select(t => t.ToResponse()));
    }
    internal static async Task<IResult> HandleGetTicketById(int id, ITicketTypeService ttService, ILogger<TicketTypeEndpointsLoggerCategory> logger, CancellationToken ct)
    {
        const bool activeOnly = false;
        logger.GetTicketTypeByIdRequested(id, activeOnly);

        var ticketType = await ttService.GetByIdAsync(id, ct);
        if (ticketType == null)
        {
            logger.GetTicketTypeByIdNotFound(id, activeOnly);
            return Results.NotFound();
        }

        logger.GetTicketTypeByIdSucceeded(id, activeOnly);
        return Results.Ok(ticketType.ToResponse());
    }
    internal static async Task<IResult> HandleCreateNew(CreateTicketTypeRequest req, ITicketTypeService ttService, ILogger<TicketTypeEndpointsLoggerCategory> logger, CancellationToken ct)
    {
        var ticketType = await ttService.CreateAsync(req, ct);
        if (ticketType == null)
            return Results.Problem();

        return Results.Created($"{ticketType.Id}", ticketType.ToResponse());
    }
    internal static async Task<IResult> HandleGetAllActive(ITicketTypeService ttService, ILogger<TicketTypeEndpointsLoggerCategory> logger, CancellationToken ct)
    {
        const bool activeOnly = true;
        logger.ListTicketTypesRequested(activeOnly);

        var activeTicketTypes = await ttService.GetAllActiveAsync(ct);

        if (activeTicketTypes == null || !activeTicketTypes.Any())
        {
            logger.ListTicketTypesNotFound(activeOnly);
            return Results.NotFound("Could not find any ticket types.");
        }

        logger.ListTicketTypesSucceeded(activeTicketTypes.Count, activeOnly);

        return Results.Ok(activeTicketTypes.Select(t => t.ToResponse()));
    }
    internal static async Task<IResult> HandleGetActiveTicketById(int id, ITicketTypeService ttService, ILogger<TicketTypeEndpointsLoggerCategory> logger, CancellationToken ct)
    {
        const bool activeOnly = true;
        logger.GetTicketTypeByIdRequested(id, activeOnly);

        var ticketType = await ttService.GetActiveByIdAsync(id, ct);
        if (ticketType == null)
        {
            logger.GetTicketTypeByIdNotFound(id, activeOnly);
            return Results.NotFound();
        }

        logger.GetTicketTypeByIdSucceeded(id, activeOnly);
        return Results.Ok(ticketType.ToResponse());
    }
    internal static async Task<IResult> HandleDelete(int id, ITicketTypeService ttService, ILogger<TicketTypeEndpointsLoggerCategory> logger, CancellationToken ct)
    {
        const string action = "Deactivated";
        logger.TicketTypeStatusChangeRequested(id, action);


        var isSuccess = await ttService.DeactivateAsync(id, ct);
        if (!isSuccess)
        {
            logger.TicketTypeStatusChangeNotFound(id, action);
            return Results.NotFound();
        }

        logger.TicketTypeStatusChangeSucceeded(id, action);
        return Results.Ok($"Successfully deactivated ticket. {id}");
    }
    internal static async Task<IResult> HandleActivate(int id, ITicketTypeService ttService, ILogger<TicketTypeEndpointsLoggerCategory> logger, CancellationToken ct)
    {
        const string action = "Activated";
        logger.TicketTypeStatusChangeRequested(id, action);

        var success = await ttService.ActivateAsync(id, ct);
        if (!success)
        {
            logger.TicketTypeStatusChangeNotFound(id, action);
            return Results.NotFound();
        }

        logger.TicketTypeStatusChangeSucceeded(id, action);
        return Results.NoContent();
    }
}
