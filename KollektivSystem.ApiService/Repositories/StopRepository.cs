using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using Microsoft.EntityFrameworkCore;


namespace KollektivSystem.ApiService.Repositories
{

    public class StopRepository : RepositoryBase<Stop, int>, IStopRepository
    {
        private readonly DbContext _context;

        public StopRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task AddAsync(Stop entity, CancellationToken ct = default)
        {
            await _context.Set<Stop>().AddAsync(entity, ct);
        }

        public async Task<Stop?> FindAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<Stop>().FindAsync(new object[] { id }, ct);
        }

        public async Task<IEnumerable<Stop>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Set<Stop>().ToListAsync(ct);
        }

        public void Update(Stop entity)
        {
            _context.Set<Stop>().Update(entity);
        }

        public void Remove(Stop entity)
        {
            _context.Set<Stop>().Remove(entity);
        }

        public IQueryable<Stop> Query()
        {
            return _context.Set<Stop>().AsQueryable();
        }
    }
}
