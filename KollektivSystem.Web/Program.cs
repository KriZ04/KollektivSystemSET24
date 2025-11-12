using KollektivSystem.Web;
using KollektivSystem.Web.Components;
using KollektivSystem.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ITokenStore, TokenStore>();
builder.Services.AddScoped<AuthState>();

builder.Services.AddOutputCache();




//builder.Services.AddHttpClient<WeatherApiClient>(client =>
//    {
//        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
//        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
//        client.BaseAddress = new("https+http://apiservice");
//    });

builder.Services.AddHttpClient<AuthApiClient>(client =>
    {
        client.BaseAddress = new("https://localhost:7445");
    });

builder.Services.AddHttpClient<ProfileClient>(c =>
{
    c.BaseAddress = new("https+http://apiservice");
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/login/redirect", (HttpContext ctx, IConfiguration cfg) =>
{
    var api = (cfg["Api:BaseUrl"] ?? "https://localhost:7445").TrimEnd('/');
    var callback = $"{ctx.Request.Scheme}://{ctx.Request.Host}/auth/callback";
    var url = $"{api}/auth/login?returnUrl={Uri.EscapeDataString(callback)}";
    return Results.Redirect(url);
});


app.MapDefaultEndpoints();

app.Run();
