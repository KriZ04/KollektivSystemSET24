using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.TicketTypes;
using KollektivSystem.ApiService.Models.Mappers;
using KollektivSystem.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Net.Sockets;

namespace KollektivSystem.ApiService.Extensions.Endpoints;

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

    internal static async Task<IResult> HandleGetAll(ITicketTypeService ttService, CancellationToken ct)
    {
        var ticketTypes = await ttService.GetAllAsync(ct);
        if (ticketTypes == null)
            return Results.NotFound();


        return Results.Ok(ticketTypes.Select(t => t.ToResponse()));
    }
    internal static async Task<IResult> HandleGetTicketById(int id, ITicketTypeService ttService, CancellationToken ct)
    {
        var ticketType = await ttService.GetByIdAsync(id, ct);
        if (ticketType == null)
            return Results.NotFound();

        return Results.Ok(ticketType.ToResponse());
    }
    internal static async Task<IResult> HandleCreateNew(CreateTicketTypeRequest req, ITicketTypeService ttService, CancellationToken ct)
    {
        var ticketType = await ttService.CreateAsync(req, ct);
        if (ticketType == null)
            return Results.Problem();

        return Results.Created($"{ticketType.Id}", ticketType.ToResponse());
    }
    internal static async Task<IResult> HandleGetAllActive(ITicketTypeService ttService, CancellationToken ct)
    {
        var activeTicketTypes = await ttService.GetAllActiveAsync(ct);

        if (activeTicketTypes == null)
            return Results.NotFound("Could not find any ticket types.");

        return Results.Ok(activeTicketTypes.Select(t => t.ToResponse()));
    }
    internal static async Task<IResult> HandleGetActiveTicketById(int id, ITicketTypeService ttService, CancellationToken ct)
    {
        var ticketType = await ttService.GetActiveByIdAsync(id, ct);
        if (ticketType == null)
            return Results.NotFound();
        
        return Results.Ok(ticketType.ToResponse());
    }
    internal static async Task<IResult> HandleDelete(int id, ITicketTypeService ttService, CancellationToken ct)
    {
        var isSuccess = await ttService.DeactivateAsync(id, ct);
        if(!isSuccess) 
            return Results.NotFound();
        return Results.Ok($"Successfully deactivated ticket. {id}");
    }
    internal static async Task<IResult> HandleActivate(int id, ITicketTypeService ttService, CancellationToken ct)
    {
        var success = await ttService.ActivateAsync(id, ct);
        if (!success)
            return Results.NotFound();

        return Results.NoContent();
    }
}
