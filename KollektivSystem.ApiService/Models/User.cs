using KollektivSystem.ApiService.Models.Enums;

namespace KollektivSystem.ApiService.Models;

public class User
{
    public int Id { get; set; }
    public required string display_name { get; set; }
    public Role Role { get; set; } = Role.None;
    public string provider {  get; set; }
    public string sub {  get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public DateTime last_login { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public int role_id { get; set; }
}
