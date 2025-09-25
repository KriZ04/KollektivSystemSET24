using Microsoft.AspNetCore.Identity;

namespace KollektivSystem.ApiService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public Role Role { get; set; } = Role.None;
    }
}
