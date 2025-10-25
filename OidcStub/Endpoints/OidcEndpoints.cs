using Microsoft.AspNetCore.Builder;
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

            group.MapGet("/login", (HttpRequest req, IOptionsSnapshot<OidcOptions> opt) =>
            {
                var clientId = req.Query["client_id"].ToString();
                var redirectUri = req.Query["redirect_uri"].ToString();
                var state = req.Query["state"].ToString();

                var items = string.Join("", opt.Value.Personas.Select(p =>
                    $"<li><a href=\"/oidc/authorize?client_id={Uri.EscapeDataString(clientId)}" +
                    $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                    $"&state={Uri.EscapeDataString(state)}" +
                    $"&persona={Uri.EscapeDataString(p.Key)}\">{System.Net.WebUtility.HtmlEncode(p.Name)}</a></li>"
                ));

                var html = $"""
                    <html><body>
                        <h1>Choose Login (OIDC STUB)</h1>
                        <ul>{items}</ul>
                    </body></html>
                    """;

                return Results.Content(html, "text/html");
            });

            group.MapGet("/authorize", (string client_id, string redirect_uri, string state, string? scope, IMemoryCache cache, IOptionsSnapshot<OidcOptions> opt) =>
            {
                var cfg = opt.Value;

                // Resolve persona key by client_id
                if (!cfg.ClientPersonas.TryGetValue(client_id, out var personaKey))
                    return Results.BadRequest($"No persona configured for client_id '{client_id}'.");

                var persona = cfg.Personas.FirstOrDefault(x =>
                    string.Equals(x.Key, personaKey, StringComparison.OrdinalIgnoreCase));

                if (persona is null)
                    return Results.BadRequest($"Persona '{personaKey}' not found for client_id '{client_id}'.");

                var code = Guid.NewGuid().ToString("N");
                cache.Set($"auth_code:{code}", new Identity(persona.Sub, persona.Email, persona.Name), TimeSpan.FromMinutes(2));

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
