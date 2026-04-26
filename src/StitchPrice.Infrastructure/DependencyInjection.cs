using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StitchPrice.Application.Interfaces;
using StitchPrice.Infrastructure.Persistence;
using StitchPrice.Infrastructure.Persistence.Repositories;

namespace StitchPrice.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<StitchPriceDbContext>(opts =>
            opts.UseNpgsql(
                connectionString,
                npgsql => npgsql.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName)));

        services.AddScoped<IQuoteRepository, QuoteRepository>();
        services.AddScoped<IPricingSettingsRepository, PricingSettingsRepository>();
        services.AddScoped<IProductProfileRepository, ProductProfileRepository>();

        return services;
    }
}
