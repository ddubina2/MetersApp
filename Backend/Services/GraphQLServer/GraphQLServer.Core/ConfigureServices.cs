using GraphQLServer.Core.Interfaces;
using GraphQLServer.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQLServer.Core;

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
