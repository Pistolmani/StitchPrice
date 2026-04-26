using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StitchPrice.Application.Common.Behaviors;
using StitchPrice.Domain.Pricing;

namespace StitchPrice.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // PricingEngine is stateless — singleton is safe and avoids rebuilding the rule list per request.
        services.AddSingleton(PricingEngine.CreateDefault());

        return services;
    }
}
