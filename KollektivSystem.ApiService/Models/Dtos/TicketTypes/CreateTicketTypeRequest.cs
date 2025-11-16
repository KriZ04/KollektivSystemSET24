namespace KollektivSystem.ApiService.Models.Dtos.TicketTypes
{
    public sealed class CreateTicketTypeRequest
    {
        public required string Name { get; init; }
        public required int Price { get; init; }
        public required int AliveTimeMinutes { get; init; }
    }
}
