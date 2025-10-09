using KollektivSystem.ApiService.Models.Transport;
using KollektivSystem.ApiService.Repositories;

namespace KollektivSystem.ApiService.Endpoints;

public static class RouteEndpoints
{
    public static void MapRouteEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/routes", async (IRouteRepository repo) => await repo.GetAllAsync());
        app.MapPost("/routes", async (IRouteRepository repo, Route route) =>
        {
            var result = await repo.AddAsync(route);
            return Results.Created($"/routes/{result.Id}", result);
        });
        app.MapPut("/routes/{id}", async (IRouteRepository repo, int id, Route route) =>
        {
            var result = await repo.UpdateAsync(id, route);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });
        app.MapDelete("/routes/{id}", async (IRouteRepository repo, int id) =>
        {
            var ok = await repo.DeleteAsync(id);
            return ok ? Results.NoContent() : Results.NotFound();
        });
    }
}
