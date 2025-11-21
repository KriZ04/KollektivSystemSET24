using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.TransitLines;

namespace KollektivSystem.ApiService.Services.Interfaces;

public interface ITransitLineService
{
    Task<TransitLine> CreateAsync(CreateTransitLineRequest request, CancellationToken ct);
    Task<IReadOnlyList<TransitLine>> GetAllAsync(CancellationToken ct);
    Task<TransitLine?> GetByIdAsync(int id, CancellationToken ct);
    Task<bool> UpdateAsync(int id, TransitLine updated, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}