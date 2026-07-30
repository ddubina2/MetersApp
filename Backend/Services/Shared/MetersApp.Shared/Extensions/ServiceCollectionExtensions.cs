using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace MetersApp.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureTelemetry(
        this IServiceCollection services,
        IWebHostEnvironment hostingEnvironment,
        string serviceName,
        string meterName)
    {
        if (hostingEnvironment.IsProduction())
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
        }

        return services;
    }
}
