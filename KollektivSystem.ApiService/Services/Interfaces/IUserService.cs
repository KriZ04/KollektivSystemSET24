using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos;
using KollektivSystem.ApiService.Models.Enums;

namespace KollektivSystem.ApiService.Services.Interfaces;

public interface IUserService
{
    public Task<UserMeDto?> GetMeByIdAsync(Guid id, CancellationToken ct);
    public Task<IReadOnlyList<UserListItemDto>> GetAllAsync(CancellationToken ct);
    public Task<bool> UpdateRoleAsync(Guid id, Role newRole,  CancellationToken ct);
    public Task<User?> GetUserById(Guid id, CancellationToken ct);
}