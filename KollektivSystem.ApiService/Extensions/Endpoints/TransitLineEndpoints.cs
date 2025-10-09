using KollektivSystem.ApiService.Models.Transport;
using KollektivSystem.ApiService.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace KollektivSystem.ApiService.Extensions.Endpoints
{
    public static class TransitLineEndpoints
    {
        public static void MapTransitLineEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/transitlines", async (TransitLine line, ITransitLineRepository repo) =>
            {
                await repo.AddAsync(line);
                await repo.SaveChanges();
                return Results.Created($"/transitlines/{line.Id}", line);
            });

            app.MapGet("/transitlines", async (ITransitLineRepository repo) =>
            {
                var lines = await repo.GetAllAsync();
                return Results.Ok(lines);
            });
        }
    }
}
