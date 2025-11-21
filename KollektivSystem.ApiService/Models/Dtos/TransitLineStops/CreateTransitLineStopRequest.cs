namespace KollektivSystem.ApiService.Models.Dtos.TransitLineStops;

public sealed class CreateTransitLineStopRequest
{
    public required int Order { get; init; }
    public required int TransitLineId { get; init; }
    public required int StopId { get; init; }
}
