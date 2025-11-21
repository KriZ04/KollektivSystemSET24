using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KollektivSystem.ApiService.Repositories;

public class TransitLineStopRepository
    : RepositoryBase<TransitLineStop, int>, ITransitLineStopRepository
{
    private readonly ApplicationDbContext _db;

    public TransitLineStopRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TransitLineStop, bool>> predicate,
        CancellationToken ct)
    {
        return await _db.Set<TransitLineStop>().AnyAsync(predicate, ct);
    }
}
