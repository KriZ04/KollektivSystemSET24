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
    private static async Task<IResult> HandleGetAll(IStopService sService, CancellationToken ct)
    {
        var stops = await sService.GetAllAsync(ct);
        if (stops == null)
            return Results.NotFound();

        return Results.Ok(stops.Select(t => t.ToResponse()));
    }

    //Get stop by ID
    private static async Task<IResult> HandleGetStopById(int id, IStopService sService, CancellationToken ct)
    {
        var stop = await sService.GetByIdAsync(id, ct);
        if (stop == null)
            return Results.NotFound();

        return Results.Ok(stop.ToResponse());
    }

    // Create new stop
    private static async Task<IResult> HandleCreateStop(CreateStopRequest req, IStopService sService, CancellationToken ct)
    {
        var stop = await sService.CreateAsync(req, ct);
        if (stop == null)
            return Results.Problem();

        return Results.Created($"{stop.Id}", stop.ToResponse());
    }

    //Delete stop
    private static async Task<IResult> HandleDeleteStop(int id, IStopService sService, CancellationToken ct)
    {
        var isSuccess = await sService.DeleteAsync(id, ct);
        if (!isSuccess)
            return Results.NotFound();

        return Results.Ok($"Successfully deleted stop. {id}");
    }
}
