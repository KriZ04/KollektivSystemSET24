using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Services.Interfaces
{
    public interface IStopService
    {
        // CRUD
        Task<IEnumerable<Stop>> GetAllAsync();
        Task<Stop?> GetByIdAsync(int id);
        Task<Stop> CreateAsync(Stop stop);
        Task<Stop?> UpdateAsync(int id, Stop stop);
        Task<bool> DeleteAsync(int id);


        // Get all stops from a transit line
        Task<IEnumerable<Stop>> GetStopsForTransitLineAsync(int transitLineId);

        // Add stop to transit line
        Task<TransitLineStop> AddStopToTransitLineAsync(int transitLineId, int stopId, int order);

        // Remove a stop from a transit line
        Task<bool> RemoveStopFromTransitLineAsync(int transitLineStopId);

        // Update the stop's order in the transit line
        Task<bool> UpdateStopOrderAsync(int transitLineStopId, int newOrder);
    }
}
