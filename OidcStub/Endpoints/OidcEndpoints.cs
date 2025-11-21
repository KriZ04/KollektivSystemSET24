using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OidcStub.Exceptions;
using OidcStub.Models;
using OidcStub.Services;
using System.Security.Claims;
using System.Text;

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
             
            group.MapGet("/authorize", (string client_id, string redirect_uri, string state, string? scope, string? persona, IMemoryCache cache, IOptionsSnapshot<OidcOptions> opt) =>
            {
                var cfg = opt.Value;

                // If persona isn't supplied, send user to the stub login UI
                if (string.IsNullOrWhiteSpace(persona))
                {
                    var pickUrl =
                        "/oidc/login"
                        + $"?client_id={Uri.EscapeDataString(client_id)}"
                        + $"&redirect_uri={Uri.EscapeDataString(redirect_uri)}"
                        + $"&state={Uri.EscapeDataString(state)}"
                        + (string.IsNullOrWhiteSpace(scope) ? "" : $"&scope={Uri.EscapeDataString(scope)}");

                    return Results.Redirect(pickUrl);
                }

                // Resolve the chosen persona key
                var p = cfg.Personas.FirstOrDefault(x =>
                    string.Equals(x.Key, persona, StringComparison.OrdinalIgnoreCase));

                if (p is null)
                    return Results.BadRequest($"Unknown login '{persona}'.");

                // Issue code and cache identity
                var code = Guid.NewGuid().ToString("N");
                cache.Set($"auth_code:{code}", new Identity(p.Sub, p.Email, p.Name), TimeSpan.FromMinutes(2));

                var location = $"{redirect_uri}"
                             + $"?code={Uri.EscapeDataString(code)}"
                             + $"&state={Uri.EscapeDataString(state)}";

                return Results.Redirect(location);
            });

            group.MapPost("/token", async (HttpRequest req, IOidcTokenService svc, CancellationToken ct) =>
            {
                try
                {
                    var form = await req.ReadFormAsync(ct);
                    var tokens = await svc.ExchangeCode(form, ct);
                    return Results.Json(tokens);
                }
                catch (OidcException ex)
                {
                    return Results.BadRequest(ex.Error);
                }
            });

            return builder;
        }
    }
}
