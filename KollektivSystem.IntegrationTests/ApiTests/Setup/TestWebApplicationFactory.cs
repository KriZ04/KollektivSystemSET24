using KollektivSystem.ApiService;
using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.IntegrationTests.ApiTests.TestClasses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KollektivSystem.IntegrationTests.ApiTests.Setup
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("AuthIntegrationTestsDb");
                });

                var authProviderDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthProvider));

                if (authProviderDescriptor != null)
                    services.Remove(authProviderDescriptor);

                services.AddSingleton<IAuthProvider, TestAuthProvider>();

                var tokenServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ITokenService));

                if (tokenServiceDescriptor != null)
                    services.Remove(tokenServiceDescriptor);
                

                services.AddSingleton<ITokenService, TestTokenService>();


                using var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
