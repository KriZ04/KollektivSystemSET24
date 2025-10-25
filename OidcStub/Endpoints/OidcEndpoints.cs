using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OidcStub.Models;

namespace OidcStub.Endpoints
{
    public static class OidcEndpoints
    {
        public static IEndpointRouteBuilder MapOidcEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("/oidc");

            group.MapGet("/personas", (IOptionsSnapshot<OidcOptions> opt) => Results.Json(opt.Value.Personas.Select(p => new { p.Key, p.Name, p.Email })));

            group.MapGet("/authorize", (string client_id, string redirect_uri, string state, string persona, IMemoryCache cache, IOptionsSnapshot<OidcOptions> opt) =>
            {
                var p = opt.Value.Personas.FirstOrDefault(x => string.Equals(x.Key, persona, StringComparison.OrdinalIgnoreCase));
                
                if (p is null) 
                    return Results.BadRequest($"Unknown login '{persona}'.");

                var code = Guid.NewGuid().ToString("N");
                cache.Set($"auth_code:{code}", new StubIdentity(p.Sub, p.Email, p.Name), TimeSpan.FromMinutes(2));
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
