using KollektivSystem.Web;
using KollektivSystem.Web.Components;
using KollektivSystem.Web.Services;

// === Velg hvilken AuthState som skal brukes (alias fjerner konflikt) ===
using AuthState = KollektivSystem.Web.Services.AuthState;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ITokenStore, TokenStore>();
builder.Services.AddScoped<AuthState>();   // <-- bruker aliaset over

builder.Services.AddOutputCache();

// HttpClients
builder.Services.AddHttpClient<AuthApiClient>(client =>
{
    client.BaseAddress = new("https://localhost:7445");
});

builder.Services.AddHttpClient<ProfileClient>(c =>
{
    c.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHttpClient<AuthTokenService>(client =>
{
    client.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHttpClient<UsersAdminClient>(c =>
{
    c.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHttpClient<TicketTypeAdminClient>(c =>
{
    c.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHttpClient<ITicketApiClient, TicketApiClient>(c =>
{
    c.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHttpClient<TransitLineAdminClient>(c =>
{
    c.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHttpClient<TransitLineStopAdminClient>(c =>
{
    c.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHttpClient<StopAdminClient>(c =>
{
    c.BaseAddress = new("https+http://apiservice");
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Enkel redirect til APIets login-endepunkt
app.MapGet("/login/redirect", (HttpContext ctx, IConfiguration cfg) =>
{
    var api = (cfg["Api:BaseUrl"] ?? "https://localhost:7445").TrimEnd('/');
    var callback = $"{ctx.Request.Scheme}://{ctx.Request.Host}/auth/callback";
    var url = $"{api}/auth/login?returnUrl={Uri.EscapeDataString(callback)}";
    return Results.Redirect(url);
});

app.MapDefaultEndpoints();
app.Run();
