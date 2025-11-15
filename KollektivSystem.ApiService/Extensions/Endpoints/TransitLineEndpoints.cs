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
            var group = app.MapGroup("/transitlines");

            //  CREATE a new transit line
            group.MapPost("", HandlePost);

            //  GET all transit lines
            group.MapGet("", HandleGet);

            //  GET one transit line by ID
            group.MapGet("{id:int}", HandleGetById);

            //  UPDATE existing transit line (your user story)
            group.MapPut("{id:int}", HandlePutById);

            // DELETE a transit line
            group.MapDelete("{id:int}", HandleDeleteById);
        }
        internal static async Task<IResult> HandlePost(TransitLine line, ITransitLineService service)
        {
            var created = await service.CreateAsync(line);
            return Results.Created($"/transitlines/{created.Id}", created);
        }
        internal static async Task<IResult> HandleGet(ITransitLineService service)
        {
            var lines = await service.GetAllAsync();
            return Results.Ok(lines);
        }
        internal static async Task<IResult> HandleGetById(int id, ITransitLineService service)
        {
            var line = await service.GetByIdAsync(id);
            return line is not null ? Results.Ok(line) : Results.NotFound();
        }
        internal static async Task<IResult> HandlePutById(int id, TransitLine updated, ITransitLineService service)
        {
            var success = await service.UpdateAsync(id, updated);
            return success ? Results.NoContent() : Results.NotFound();
        }
        internal static async Task<IResult> HandleDeleteById(int id, ITransitLineService service)
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        }
    }
}
