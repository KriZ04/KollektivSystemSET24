using KollektivSystem.ApiService.Models.Transport;
using KollektivSystem.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;

namespace KollektivSystem.ApiService.Extensions.Endpoints
{
    public static class TicketsEndpoints
    {
        public static IEndpointRouteBuilder MapTicketEndpoints(this IEndpointRouteBuilder app)
        {
            // creat a grouped route with swagger tag "Tickets"
            var group = app.MapGroup("/api/tickets").WithTags("Tickets");


            // GET all tickets
            // returns all tickets from the system
            group.MapGet("/", async (ITicketService service) =>
            {
                var tickets = await service.GetAllAsync();
                return Results.Ok(tickets);
            });

            // GET ticket by ID
            // returns a single ticket by ID 
            group.MapGet("/{id:int}", async (int id, ITicketService service) =>
            {
                var ticket = await service.GetByIdAsync(id);
                return ticket is not null ? Results.Ok(ticket) : Results.NotFound();
            });

            // POST create a new ticket
            group.MapPost("/", async (Tickets ticket, ITicketService service) =>
            {
                var createdTicket = await service.CreateAsync(ticket);
                return Results.Created($"/api/tickets/{createdTicket.Id}", createdTicket);
            });

            // Update ticket an existing ticket by ID
            group.MapPut("/{id:int}", async (int id, Tickets update, ITicketService service) =>
            {
                var success = await service.UpdateAsync(id, update);
                return success ? Results.NoContent() : Results.NotFound();
            });

            // DELETE /api/tickets/{id}
            // Deletes a ticket
            group.MapDelete("/{id:int}", async (int id, ITicketService service) =>
            {
                var success = await service.DeleteAsync(id);
                return success ? Results.NoContent() : Results.NotFound();
            });

            return app;



        }



    }
}
