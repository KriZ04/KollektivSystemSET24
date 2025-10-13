using KollektivSystem.ApiService.Models.Transport;

public interface ITransitLineService
{
    Task<TransitLine> CreateAsync(TransitLine line);
    Task<IEnumerable<TransitLine>> GetAllAsync();
    Task<TransitLine?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, TransitLine line);
    Task<bool> DeleteAsync(int id);
}
