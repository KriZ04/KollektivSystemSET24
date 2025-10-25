using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using OidcStub.Models;

namespace OidcStub.Endpoints
{
    public static class OidcEndpoints
    {
        public static IEndpointRouteBuilder MapOidcEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("/oidc");

            group.MapGet("/personas", async (ApplicationDbContext db, CancellationToken ct) =>
            {
                var personas = await db.Personas
                    .AsNoTracking()
                    .Select(p => new { p.Key, p.Name, p.Email, p.Role })
                    .ToListAsync(ct);
                return Results.Json(personas);
            });

            group.MapGet("/authorize", (string client_id, string redirect_uri, string state, string? scope, string? email, string? name, IMemoryCache cache) =>
            {
                if (string.IsNullOrWhiteSpace(name))
                    return Results.BadRequest("'name' query parameter is required.");

                var code = Guid.NewGuid().ToString("N");
                var sub = email?.ToLowerInvariant() ?? name.ToLowerInvariant();

                cache.Set(
                    $"auth_code:{code}",
                    new StubIdentity(sub, email, name),
                    TimeSpan.FromMinutes(2));

                return Results.Redirect($"{redirect_uri}?code={code}&state={state}");
            });


            //group.MapPost("/token", async (HttpRequest req, IMemoryCache cache, IAuthProvider provider) =>
            //{
            //    var form = await req.ReadFormAsync();
            //    var code = form["code"].ToString();
            //    if (!cache.TryGetValue($"auth_code:{code}", out StubIdentity? ident) || ident is null)
            //        return Results.BadRequest("invalid_code");

            //    var tokens = ((MockAuthProvider)provider).IssueTokensFor(ident);
            //    return Results.Json(new { id_token = tokens.IdToken, access_token = tokens.AccessToken, token_type = "Bearer", expires_in = 3600 });
            //});

            return builder;
        }
    }
}
