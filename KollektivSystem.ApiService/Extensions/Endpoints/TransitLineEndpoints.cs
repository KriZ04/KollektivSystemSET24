using KollektivSystem.ApiService.Models.Transport;
using KollektivSystem.ApiService.Repositories;

namespace KollektivSystem.ApiService.Endpoints;

public static class TransitLineEndpoints
{
    public static void MapRouteEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/routes", async (ITransitLineRepository repo) => await repo.GetAllAsync());
        app.MapPost("/routes", async (ITransitLineRepository repo, Route route) =>
        {
            var result = await repo.AddAsync(route);
            return Results.Created($"/routes/{result.Id}", result);
        });
        app.MapPut("/routes/{id}", async (ITransitLineRepository repo, int id, Route route) =>
        {
            var result = await repo.UpdateAsync(id, route);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });
        app.MapDelete("/routes/{id}", async (ITransitLineRepository repo, int id) =>
        {
            var ok = await repo.DeleteAsync(id);
            return ok ? Results.NoContent() : Results.NotFound();
        });
    }
}
