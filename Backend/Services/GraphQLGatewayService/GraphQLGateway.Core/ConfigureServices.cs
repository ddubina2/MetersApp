namespace GraphQLGateway.Core;

using GraphQLGateway.Core.Interfaces;
using GraphQLGateway.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureCoreServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IAirQualityService, AirQualityService>();
        services.AddScoped<IEnergyService, EnergyService>();
        services.AddScoped<IMotionService, MotionService>();

        return services;
    }
}
