namespace KollektivSystem.ApiService.Infrastructure;

public record ApiClaims(Guid UserId, string Role, string Provider, string Sub);
public interface IJwtIssuer
{
    string Issue(ApiClaims claims);
}
