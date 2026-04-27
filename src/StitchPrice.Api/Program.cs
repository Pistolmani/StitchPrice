using Scalar.AspNetCore;
using StitchPrice.Api.Middleware;
using StitchPrice.Application;
using StitchPrice.Infrastructure;
using StitchPrice.Infrastructure.Persistence;
using StitchPrice.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' is not configured.");

builder.Services
    .AddApplication()
    .AddInfrastructure(connectionString);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(opts =>
    opts.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<StitchPriceDbContext>();
    await DatabaseSeeder.SeedAsync(db);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("Frontend");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

// Exposed for WebApplicationFactory in integration tests
public partial class Program;
