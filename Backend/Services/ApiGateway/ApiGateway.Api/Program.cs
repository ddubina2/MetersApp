using MetersApp.Shared.Extensions;
using MetersApp.Shared.Middlewares;
using MetersApp.Shared.Options;
using Serilog;

namespace ApiGateway.Api;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .ConfigureMetersAppLogging()
            .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSerilog();
            builder.Services.ConfigureTelemetry(
                builder.Environment,
                "api-gateway",
                "ApiGateway.Metrics");

            builder.Services.Configure<CorsOptions>(builder.Configuration.GetSection(nameof(CorsOptions)));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    var corsOptions = builder.Configuration.GetSection(nameof(CorsOptions)).Get<CorsOptions>();
                    policy.WithOrigins(corsOptions?.AllowedOrigins ?? [])
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSerilogRequestLogging();
            app.UseCors("AllowFrontend");
            app.UseWebSockets();
            app.MapReverseProxy();

            if (app.Environment.IsProduction())
            {
                app.MapPrometheusScrapingEndpoint();
            }

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
