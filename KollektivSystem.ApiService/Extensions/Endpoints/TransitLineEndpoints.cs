using KollektivSystem.ApiService.Models.Transport;
using KollektivSystem.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace KollektivSystem.ApiService.Extensions.Endpoints
{
    public static class TransitLineEndpoints
    {
        public static void MapTransitLineEndpoints(this IEndpointRouteBuilder app)
        {
            //  CREATE a new transit line
            app.MapPost("/transitlines", async (TransitLine line, ITransitLineService service) =>
            {
                var created = await service.CreateAsync(line);
                return Results.Created($"/transitlines/{created.Id}", created);
            });

            //  GET all transit lines
            app.MapGet("/transitlines", async (ITransitLineService service) =>
            {
                var lines = await service.GetAllAsync();
                return Results.Ok(lines);
            });

            //  GET one transit line by ID
            app.MapGet("/transitlines/{id:int}", async (int id, ITransitLineService service) =>
            {
                var line = await service.GetByIdAsync(id);
                return line is not null ? Results.Ok(line) : Results.NotFound();
            });

            //  UPDATE existing transit line (your user story)
            app.MapPut("/transitlines/{id:int}", async (int id, TransitLine updated, ITransitLineService service) =>
            {
                var success = await service.UpdateAsync(id, updated);
                return success ? Results.NoContent() : Results.NotFound();
            });

            // DELETE a transit line
            app.MapDelete("/transitlines/{id:int}", async (int id, ITransitLineService service) =>
            {
                var success = await service.DeleteAsync(id);
                return success ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}
