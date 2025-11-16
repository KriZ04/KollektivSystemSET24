namespace KollektivSystem.ApiService.Models
{
    public class TicketInfo
    {
        public int Id { get; init; }

        public required string Name { get; init; }

        public required int Price { get; init; }

        public required TimeSpan AliveTime { get; init; }
    }
}
