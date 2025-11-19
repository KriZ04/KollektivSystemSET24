using AutoMapper;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.TransitLineStops;
using KollektivSystem.ApiService.Repositories.Interfaces;
using KollektivSystem.ApiService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KollektivSystem.ApiService.Services
{
    public sealed class TransitLineStopService : ITransitLineStopService
    {
        private readonly ITransitLineStopRepository _repo;
        private readonly IMapper _mapper;

        public TransitLineStopService(
            ITransitLineStopRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TransitLineStopResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(id, cancellationToken);
            return entity is null ? null : _mapper.Map<TransitLineStopResponse>(entity);
        }

        public async Task<IEnumerable<TransitLineStopResponse>> GetByTransitLineIdAsync(
            int transitLineId,
            CancellationToken cancellationToken)
        {
            var query = _repo
                .Query()
                .Where(x => x.TransitLineId == transitLineId)
                .OrderBy(x => x.Order)
                .Include(x => x.Stop)
                .Include(x => x.TransitLine);

            var result = await query.ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<TransitLineStopResponse>>(result);
        }

        public async Task<IEnumerable<TransitLineStopResponse>> GetByStopIdAsync(
            int stopId,
            CancellationToken cancellationToken)
        {
            var query = _repo
                .Query()
                .Where(x => x.StopId == stopId)
                .OrderBy(x => x.Order)
                .Include(x => x.Stop)
                .Include(x => x.TransitLine);

            var result = await query.ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<TransitLineStopResponse>>(result);
        }

        public async Task<TransitLineStopResponse> CreateAsync(
            CreateTransitLineStopRequest request,
            CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TransitLineStop>(request);

            var created = await _repo.AddAsync(entity, cancellationToken);

            return _mapper.Map<TransitLineStopResponse>(created);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(id, cancellationToken);
            if (entity is null)
                return false;

            await _repo.DeleteAsync(entity, cancellationToken);
            return true;
        }
    }
}
