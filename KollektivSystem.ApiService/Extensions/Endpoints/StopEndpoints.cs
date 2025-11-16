using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;

namespace KollektivSystem.ApiService.Extensions.Endpoints
{
    public static class StopEndpoints
    {
        public static IEndpointRouteBuilder MapStopEndpoints(this IEndpointRouteBuilder app)
        {
            // Create a grouped route: /api/stops
            var group = app.MapGroup("/api/stops").WithTags("Stops");

            
            // GET /api/stops
            // Returns all stops
           
            group.MapGet("/", async (IStopService service) =>
            {
                var stops = await service.GetAllAsync();
                return Results.Ok(stops);
            });

            
            // GET /api/stops/{id}
            // Returns a single stop by ID
            
            group.MapGet("/{id:int}", async (int id, IStopService service) =>
            {
                var stop = await service.GetByIdAsync(id);
                return stop is not null ? Results.Ok(stop) : Results.NotFound();
            });

           
            // POST /api/stops
            // Creates a new stop
          
            group.MapPost("/", async (Stop stop, IStopService service) =>
            {
                var created = await service.CreateAsync(stop);
                return Results.Created($"/api/stops/{created.Id}", created);
            });

            
            // PUT /api/stops/{id}
            // Updates an existing stop
           
            group.MapPut("/{id:int}", async (int id, Stop update, IStopService service) =>
            {
                var success = await service.UpdateAsync(id, update);
                return success ? Results.NoContent() : Results.NotFound();
            });

            
            // DELETE /api/stops/{id}
            // Deletes a stop
            
            group.MapDelete("/{id:int}", async (int id, IStopService service) =>
            {
                var success = await service.DeleteAsync(id);
                return success ? Results.NoContent() : Results.NotFound();
            });

            return app;
        }



    }
}
