using KollektivSystem.ApiService.Models.Dtos.TransitLineStops;

namespace KollektivSystem.ApiService.Services.Interfaces
{
    public interface ITransitLineStopService
    {
        Task<TransitLineStopResponse?> GetByIdAsync(int id, CancellationToken ct);
        Task<IEnumerable<TransitLineStopResponse>> GetByTransitLineIdAsync(int transitLineId, CancellationToken ct);
        Task<IEnumerable<TransitLineStopResponse>> GetByStopIdAsync(int stopId, CancellationToken ct);
        Task<TransitLineStopResponse> CreateAsync(CreateTransitLineStopRequest request, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
