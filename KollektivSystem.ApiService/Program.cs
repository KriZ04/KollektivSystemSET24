using KollektivSystem.ApiService.Extensions.Endpoints;
using KollektivSystem.ApiService.Extensions.ServiceExtensions;
using KollektivSystem.ApiService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();


builder.AddSqlServerDbContext<ApplicationDbContext>(connectionName: "database");
builder.Services.AddRepositories();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    db.Database.Migrate();

    //Example to add something to db :)
    //db.Add(new User { FirstName = "Christian", Role = KollektivSystem.ApiService.Models.Enums.Role.Developer });
    //await db.SaveChangesAsync();
}


// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapUserEndpoints();


app.MapDefaultEndpoints();

app.Run();