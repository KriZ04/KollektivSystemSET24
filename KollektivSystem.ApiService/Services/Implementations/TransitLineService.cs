using KollektivSystem.ApiService.Models.Transport;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Services.Interfaces;

namespace KollektivSystem.ApiService.Services.Implementations
{
    public class TransitLineService : ITransitLineService
    {
        private readonly ITransitLineRepository _repo;

        public TransitLineService(ITransitLineRepository repo)
        {
            _repo = repo;
        }

        public async Task<TransitLine> CreateAsync(TransitLine line)
        {
            await _repo.AddAsync(line);
            await _repo.SaveChanges();
            return line;
        }

        public async Task<IEnumerable<TransitLine>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<TransitLine?> GetByIdAsync(int id)
        {
            return await _repo.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(int id, TransitLine updated)
        {
            var existing = await _repo.FindAsync(id);
            if (existing == null)
                return false;

            existing.Name = updated.Name;
            existing.Stops = updated.Stops;

            _repo.Update(existing);
            await _repo.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.FindAsync(id);
            if (existing == null)
                return false;

            _repo.Remove(existing);
            await _repo.SaveChanges();
            return true;
        }
    }
}
