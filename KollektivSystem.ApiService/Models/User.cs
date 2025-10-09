using KollektivSystem.ApiService.Models.Enums;

namespace KollektivSystem.ApiService.Models;

public class User
{
    public Guid Id { get; set; }
    public required string DisplayName { get; set; }
    public Role Role { get; set; } = Role.None;
    public string Provider {  get; set; }
    public string Sub {  get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public string? Email { get; set; } = null;
}
