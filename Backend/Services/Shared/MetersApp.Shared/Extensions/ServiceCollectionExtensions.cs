namespace MetersApp.Shared.Extensions;

using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureTelemetry(
        this IServiceCollection services,
        string serviceName,
        string meterName)
    {
        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                    .AddAspNetCoreInstrumentation()
                    .AddMeter(meterName)
                    .AddPrometheusExporter();
            });

        return services;
    }
}
