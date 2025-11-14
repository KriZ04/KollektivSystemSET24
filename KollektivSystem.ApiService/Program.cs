using KollektivSystem.ApiService.Extensions.Endpoints;
using KollektivSystem.ApiService.Extensions.ServiceExtensions;
using KollektivSystem.ApiService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using OidcStub.Endpoints;
using OidcStub.Extensions;
using System.Security.Claims;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add service defaults & Aspire client integrations.
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddProblemDetails();


        builder.AddSqlServerDbContext<ApplicationDbContext>(connectionName: "database");
        builder.Services.AddRepositories();
        builder.Services.AddAuths(builder.Configuration);

        builder.Services.AddOidcStub(builder.Configuration);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "API", Version = "v1" });

            // JWT Bearer in Swagger
            c.AddSecurityDefinition("Bearer", new()
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter: Bearer {your JWT}"
            });
            c.AddSecurityRequirement(new()
            {
        {
            new() { Reference = new()
            { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                c.RoutePrefix = "";
            });
        }

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (db.Database.IsRelational())
            {
                db.Database.Migrate();
            }
            else
            {
                db.Database.EnsureCreated();
            }
        }


        // Configure the HTTP request pipeline.
        app.UseExceptionHandler();
        app.Use(async (context, next) =>
        {
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown";

            var logger = context.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("UserScope");

            using (logger.BeginScope(new Dictionary<string, object>
            {
                ["user_id"] = userId
            }))
            {
                await next();
            }
        });
        app.MapUserEndpoints();
        app.MapAuthEndpoints();
        app.MapOidcEndpoints();

        app.MapDefaultEndpoints();

        app.Run();
    }
}