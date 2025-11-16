using KollektivSystem.ApiService.Models.Enums;
using System.Security.Claims;

namespace KollektivSystem.ApiService.Models;

public class User
{
    public Guid Id { get; set; }
    public required string DisplayName { get; set; }
    public Role Role { get; set; } = Role.Customer;
    public AuthProvider Provider {  get; set; }
    public required string Sub {  get; set; }
    public DateTime? CreatedAt { get; init;  } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; } = DateTime.UtcNow;
    public string? Email { get; set; } = null;
    public IReadOnlyCollection<PurchasedTicket> PurchasedTickets { get; set; } = [];

    public void UpdateLogin()
    {
        LastLogin = DateTime.UtcNow;
    }
}
public static class UserExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(id))
            throw new UnauthorizedAccessException("");

        if (!Guid.TryParse(id, out var guid))
            throw new UnauthorizedAccessException("");

        return guid;
    }

}
