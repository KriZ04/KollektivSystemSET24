using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;

namespace KollektivSystem.ApiService.Extensions.Endpoints;

public static class TicketTypeEndpoints
{
    public static IEndpointRouteBuilder MapTicketTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tickets-type");

        group.MapGet("", HandleGetAllActive);
        group.MapGet("{id:int}", HandleGetActiveTicketById);

        var admin = group.MapGroup("/admin").RequireAuthorization("Staff");
        admin.MapGet("", HandleGetAll).RequireAuthorization("Staff");
        admin.MapGet("{id:int}", HandleGetTicketById).RequireAuthorization("Staff");
        admin.MapPost("", HandleCreateNew).RequireAuthorization("Staff");
        admin.MapDelete("{id:int}", HandleDelete).RequireAuthorization("Staff");

        return app;

    }

    private static async Task<IResult> HandleGetAll(HttpContext context)
    {
        throw new NotImplementedException();
    }
    private static async Task<IResult> HandleGetTicketById(HttpContext context)
    {
        throw new NotImplementedException();
    }
    private static async Task<IResult> HandleCreateNew(HttpContext context)
    {
        throw new NotImplementedException();
    }
    private static async Task<IResult> HandleGetAllActive(HttpContext context)
    {
        throw new NotImplementedException();
    }
    private static async Task<IResult> HandleGetActiveTicketById(HttpContext context)
    {
        throw new NotImplementedException();
    }
    private static async Task<IResult> HandleDelete(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
