namespace KollektivSystem.ApiService.Models.Dtos.Stops;

public sealed class CreateStopRequest
{
    public required string Name { get; init; }
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
}
