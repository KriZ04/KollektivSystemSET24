namespace KollektivSystem.Web;

public class AuthState
{
    public string? Token { get; private set; }
    public string? Role { get; private set; }
    public bool IsAdmin => string.Equals(Role, "Admin", StringComparison.OrdinalIgnoreCase);

    public void Set(string token, string role)
    {
        Token = token;
        Role = role;
    }

    public void Clear()
    {
        Token = null;
        Role = null;
    }
}
