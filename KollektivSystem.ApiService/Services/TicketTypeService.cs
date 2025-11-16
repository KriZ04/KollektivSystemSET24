using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.TicketTypes;
using KollektivSystem.ApiService.Services.Interfaces;

namespace KollektivSystem.ApiService.Services
{
    public sealed class TicketTypeService : ITicketTypeService
    {
        public Task<TicketType> CreateAsync(CreateTicketTypeRequest request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeactivateAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<TicketType?> GetActiveByIdAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TicketType>> GetAllActiveAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TicketType>> GetAllAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<TicketType?> GetByIdAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
