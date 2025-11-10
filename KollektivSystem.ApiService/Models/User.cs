using KollektivSystem.ApiService.Models.Enums;

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

    public void UpdateLogin()
    {
        LastLogin = DateTime.UtcNow;
    }
}
