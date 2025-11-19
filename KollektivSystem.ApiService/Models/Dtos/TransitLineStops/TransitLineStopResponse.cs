namespace KollektivSystem.ApiService.Models.Dtos.TransitLineStops
{
    public sealed class TransitLineStopResponse
    {
        public int Id { get; init; }
        public int Order { get; init; }
        public int TransitLineId { get; init; }
        public int StopId { get; init; }
    }
}
