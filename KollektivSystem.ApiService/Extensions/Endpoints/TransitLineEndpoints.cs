using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.ApiService.Models.Dtos.TransitLines;
using KollektivSystem.ApiService.Models.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using OidcStub.Models;

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

        // Create new transit line
        internal static async Task<IResult> HandlePost(CreateTransitLineRequest req, ITransitLineService tlService, CancellationToken ct)
        {
            var createdLine = await tlService.CreateAsync(req, ct);
            if (createdLine == null)
                return Results.Problem();

            return Results.Created($"{createdLine.Id}", createdLine.ToResponse());
        }

        // Get all transit lines
        internal static async Task<IResult> HandleGet(ITransitLineService tlService, CancellationToken ct)
        {
            var lines = await tlService.GetAllAsync(ct);
            if (lines == null) 
                return Results.NotFound();

            return Results.Ok(lines.Select(tl => tl.ToResponse()));
        }

        // Get transit line by id
        internal static async Task<IResult> HandleGetById(int id, ITransitLineService tlService, CancellationToken ct)
        {
            var line = await tlService.GetByIdAsync(id, ct);
            return line is not null ? Results.Ok(line) : Results.NotFound();
        }

        // Update transit line by id
        internal static async Task<IResult> HandlePutById(int id, TransitLine updated, ITransitLineService tlService, CancellationToken ct)
        {
            var success = await tlService.UpdateAsync(id, updated, ct);
            return success ? Results.NoContent() : Results.NotFound();
        }

        // Delete transit line by id
        internal static async Task<IResult> HandleDeleteById(int id, ITransitLineService tlService, CancellationToken ct)
        {
            var isSuccess = await tlService.DeleteAsync(id, ct);
            if (!isSuccess) 
                return Results.NotFound();

            return Results.Ok($"Successfully deleted transit line. {id}");
        }
    }
}
