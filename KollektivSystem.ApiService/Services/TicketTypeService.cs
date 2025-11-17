using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.TicketTypes;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services.Interfaces;
using Microsoft.Identity.Client;

namespace KollektivSystem.ApiService.Services
{
    public sealed class TicketTypeService : ITicketTypeService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<TicketTypeService> _logger;

        public TicketTypeService(IUnitOfWork uow, ILogger<TicketTypeService> logger)
        {
            _uow = uow;
            _logger = logger;
        }
        public async Task<TicketType> CreateAsync(CreateTicketTypeRequest request, CancellationToken ct)
        {
            var aliveTime = TimeSpan.FromMinutes(request.AliveTimeMinutes);
            var ticketType = new TicketType
            {
                Name = request.Name,
                Price = request.Price,
                AliveTime = aliveTime
            };

            await _uow.TicketTypes.AddAsync(ticketType, ct);
            await _uow.SaveChangesAsync(ct);
            return ticketType;
        }

        public async Task<bool> DeactivateAsync(int id, CancellationToken ct)
        {
            var ticketType = await _uow.TicketTypes.FindAsync(id, ct);
            if (ticketType == null)
                return false;
            ticketType.IsActive = false;
            _uow.TicketTypes.Update(ticketType);
            await _uow.SaveChangesAsync(ct);
            return true;
        }
        public async Task<bool> ActivateAsync(int id, CancellationToken ct)
        {
            var ticketType = await _uow.TicketTypes.FindAsync(id, ct);
            if (ticketType == null)
                return false;

            ticketType.IsActive = true;
            _uow.TicketTypes.Update(ticketType);
            await _uow.SaveChangesAsync(ct);

            return true;
        }
        public async Task<TicketType?> GetByIdAsync(int id, CancellationToken ct)
        {
            var ticketType = await _uow.TicketTypes.FindAsync(id, ct);
            return ticketType;
        }
        public async Task<TicketType?> GetActiveByIdAsync(int id, CancellationToken ct)
        {
            var ticketType = await GetByIdAsync(id, ct);
            if (ticketType == null || ticketType.IsActive == false)
                return null;

            return ticketType;
        }
        public async Task<IReadOnlyList<TicketType>> GetAllAsync(CancellationToken ct)
        {
            var ticketTypes = await _uow.TicketTypes.GetAllAsync(ct);
            return ticketTypes;
        }

        

        public async Task<IReadOnlyList<TicketType>> GetAllActiveAsync(CancellationToken ct)
        {
            var ticketTypes = await GetAllAsync(ct);
            return ticketTypes.Where(t => t.IsActive).ToList();
        }
    }
}
