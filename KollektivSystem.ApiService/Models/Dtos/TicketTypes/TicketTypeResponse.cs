namespace KollektivSystem.ApiService.Models.Dtos.TicketTypes
{
    public sealed class TicketTypeResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public int Price { get; init; }
        public TimeSpan AliveTime { get; init; }
        public bool IsActive { get; init; }
    }
}
