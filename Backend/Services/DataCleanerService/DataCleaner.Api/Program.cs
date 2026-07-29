using DataCleaner.Api.Models;
using DataCleaner.Core;
using DataCleaner.Core.Interfaces;
using DataCleaner.Data;
using MetersApp.Shared.Extensions;
using MetersApp.Shared.Middlewares;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace DataCleaner.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSerilog();
            builder.Services.ConfigureTelemetry(
                builder.Environment,
                "data-cleaner",
                "DataCleaner.Metrics");

            builder.Services.AddDbContext<DataCleanerDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.ConfigureCoreServices(builder.Configuration);

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSerilogRequestLogging();

            app.MapPost("/api/sensor-data/cleanup", async (
                CleanupSensorDataRequest request,
                ISensorDataCleanupService cleanupService,
                CancellationToken cancellationToken) =>
            {
                if (request.OlderThan > DateTime.UtcNow)
                {
                    return Results.BadRequest("Date cannot be in the future.");
                }

                await cleanupService.DeleteOldSensorDataAsync(request.OlderThan, cancellationToken);

                return Results.NoContent();
            });

            if (app.Environment.IsProduction())
            {
                app.MapPrometheusScrapingEndpoint();
            }

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}
