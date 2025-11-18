using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos;
using KollektivSystem.ApiService.Models.Enums;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services.Interfaces;
using System.Runtime.InteropServices;

namespace KollektivSystem.ApiService.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork uow, ILogger<UserService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<IReadOnlyList<UserListItemDto>> GetAllAsync(CancellationToken ct)
        {
            var list = await _uow.Users.GetAllAsync(ct);

            return list
                .Select(u => new UserListItemDto(
                    u.Id,
                    u.DisplayName,
                    u.Email,
                    u.Role.ToString(),
                    u.Provider.ToString(),
                    u.Sub,
                    u.LastLogin,
                    u.PurchasedTickets
            )).ToList();
        }

        public async Task<UserMeDto?> GetMeByIdAsync(Guid id, CancellationToken ct = default)
        {
            var user = await GetUserByIdAsync(id, ct);
            if (user == null)
                return null;
            
            var userDto = new UserMeDto(user.Id, user.DisplayName, user.Email, user.Role.ToString());
            return userDto;
        }

        public async Task<bool> UpdateRoleAsync(Guid id, Role newRole, CancellationToken ct)
        {
            var user = await GetUserByIdAsync(id, ct);
            if (user == null)
                return false;

            user.Role = newRole;
            user.UpdatedAt = DateTime.UtcNow;

            _uow.Users.Update(user);
            await _uow.SaveChangesAsync(ct);
            return true;

        }

        public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken ct)
        {
            var user = await _uow.Users.FindAsync(id, ct);
            if (user == null)
                return null;

            return user;
        }
    }
}
