using KollektivSystem.Web;
using KollektivSystem.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

// Http clients
builder.Services.AddHttpClient<WeatherApiClient>(c =>
{
    c.BaseAddress = new("https+http://apiservice");
});

// Bind interface til implementasjon
builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(c =>
{
    c.BaseAddress = new("https+http://apiservice");
});

// Frontend auth state
builder.Services.AddSingleton<AuthState>();

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

app.MapDefaultEndpoints();

app.Run();
