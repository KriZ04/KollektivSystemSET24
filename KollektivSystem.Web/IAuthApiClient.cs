namespace KollektivSystem.Web;

public interface IAuthApiClient
{
    Task<LoginResponse?> LoginAsync(string email, string password, CancellationToken ct = default);
}
