namespace KollektivSystem.ApiService.Models.Dtos.Tickets;

public sealed class CreateTicketTypeRequest
{
    public required string Name { get; init; }
    public required float Price { get; init; }
    public required int AliveTimeMinutes { get; init; }
}
