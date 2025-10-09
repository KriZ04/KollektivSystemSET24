using KollektivSystem.ApiService.Models.Transport;
using KollektivSystem.ApiService.Repositories;

namespace KollektivSystem.ApiService.Endpoints;

public static class TransitLineEndpoints
{
    public static void MapTransitLineEndpoints(this IEndpointRouteBuilder app) 
    {
        // Hent alle linjer
        app.MapGet("/transitlines", async (ITransitLineRepository repo) =>
            await repo.GetAllAsync());

        // Legg til ny linje
        app.MapPost("/transitlines", async (ITransitLineRepository repo, TransitLine line) =>
        {
            var result = await repo.AddAsync(line);
            return Results.Created($"/transitlines/{result.Id}", result);
        });

        // Oppdater eksisterende linje
        app.MapPut("/transitlines/{id}", async (ITransitLineRepository repo, int id, TransitLine line) =>
        {
            var result = await repo.UpdateAsync(id, line);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        // Slett en linje
        app.MapDelete("/transitlines/{id}", async (ITransitLineRepository repo, int id) =>
        {
            var ok = await repo.DeleteAsync(id);
            return ok ? Results.NoContent() : Results.NotFound();
        });
    }
}
