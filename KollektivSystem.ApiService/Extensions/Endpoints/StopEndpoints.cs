using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.Stops;
using KollektivSystem.ApiService.Models.Mappers;
using KollektivSystem.ApiService.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Net.Sockets;

namespace KollektivSystem.ApiService.Extensions.Endpoints;

public static class StopEndpoints
{
    public static IEndpointRouteBuilder MapStopEndpoints(this IEndpointRouteBuilder app)
    {
        // Create a grouped route: /api/stops
        var group = app.MapGroup("/stops");

        // CRUD for stops
        group.MapGet("", HandleGetAll);
        group.MapGet("{id:int}", HandleGetStopById);
        group.MapPost("", HandleCreateStop);
        group.MapDelete("{id:int}", HandleDeleteStop);

        return app;
    }

    // Get all stops
    private static async Task<IResult> HandleGetAll(IStopService ttService, CancellationToken ct)
    {
        var stops = await ttService.GetAllAsync(ct);
        if (stops == null)
            return Results.NotFound();

        return Results.Ok(stops.Select(t => t.ToResponse()));
    }

    //Get stop by ID
    private static async Task<IResult> HandleGetStopById(int id, IStopService ttService, CancellationToken ct)
    {
        var stop = await ttService.GetByIdAsync(id, ct);
        if (stop == null)
            return Results.NotFound();

        return Results.Ok(stop.ToResponse());
    }

    // Create new stop
    private static async Task<IResult> HandleCreateStop(CreateStopRequest req, IStopService ttService, CancellationToken ct)
    {
        var stop = await ttService.CreateAsync(req, ct);
        if (stop == null)
            return Results.Problem();

        return Results.Created($"{stop.Id}", stop.ToResponse());
    }

    //Delete stop
    private static async Task<IResult> HandleDeleteStop(int id, IStopService ttService, CancellationToken ct)
    {
        var isSuccess = await ttService.DeleteAsync(id, ct);
        if (!isSuccess)
            return Results.NotFound();

        return Results.Ok($"Successfully deleted stop. {id}");
    }
}
