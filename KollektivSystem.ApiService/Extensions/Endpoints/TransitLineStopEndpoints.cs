using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.TransitLineStops;
using KollektivSystem.ApiService.Models.Mappers;
using KollektivSystem.ApiService.Services.Interfaces;

namespace KollektivSystem.ApiService.Extensions.Endpoints;

public static class TransitLineStopEndpoints
{
    public static void MapTransitLineStopEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/transitlinestops");

        group.MapGet("/", HandleGetAll);
        group.MapGet("/{id:int}", HandleGetById);
        group.MapPost("/", HandleCreate);
        group.MapPut("/{id:int}", HandleUpdate);
        group.MapDelete("/{id:int}", HandleDelete);
    }

    internal static async Task<IResult> HandleGetAll(
        ITransitLineStopService service,
        CancellationToken ct)
    {
        var entities = await service.GetAllAsync(ct);
        var responses = entities.Select(e => e.ToResponse());
        return Results.Ok(responses);
    }

    internal static async Task<IResult> HandleGetById(
        int id,
        ITransitLineStopService service,
        CancellationToken ct)
    {
        var entity = await service.GetByIdAsync(id, ct);
        if (entity is null)
            return Results.NotFound();

        return Results.Ok(entity.ToResponse());
    }

    internal static async Task<IResult> HandleCreate(
        CreateTransitLineStopRequest request,
        ITransitLineStopService service,
        CancellationToken ct)
    {
        var entity = await service.CreateAsync(request, ct);
        if (entity == null)
            return Results.BadRequest();
        return Results.Created($"/transitlinestops/{entity.Id}", entity.ToResponse());
    }

    internal static async Task<IResult> HandleUpdate(
        int id,
        TransitLineStop updated,
        ITransitLineStopService service,
        CancellationToken ct)
    {
        var success = await service.UpdateAsync(id, updated, ct);
        return success ? Results.NoContent() : Results.NotFound();
    }

    internal static async Task<IResult> HandleDelete(
        int id,
        ITransitLineStopService service,
        CancellationToken ct)
    {
        var success = await service.DeleteAsync(id, ct);
        return success ? Results.NoContent() : Results.NotFound();
    }
}
