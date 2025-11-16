namespace KollektivSystem.ApiService.Models
{
    public class TicketType
    {
        public int Id { get; init; }

        public required string Name { get; init; }

        public required int Price { get; init; }

        public required TimeSpan AliveTime { get; init; }
        public bool IsActive { get; set; } = true;
    }
}
