using System.Text.Json.Serialization;
using MassTransit;
using MetersApp.Shared.Extensions;
using MetersApp.Shared.Middlewares;
using MetersApp.Shared.Options;
using Notifications.Api.SignalR;
using Notifications.Core.Consumers;
using Notifications.Core.Interfaces;
using Serilog;

namespace Notifications.Api;

public static class Program
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
                "notifications",
                "Notifications.Metrics");

            builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));
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

            builder.Services.AddSignalR()
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddMassTransit(x =>
            {
                var options = builder.Configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();

                x.AddConsumer<NewSensorDataEventConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(options?.Host, "/", h =>
                    {
                        h.Username(options?.UserName ?? string.Empty);
                        h.Password(options?.Password ?? string.Empty);
                    });

                    cfg.ReceiveEndpoint(e =>
                    {
                        e.ConfigureConsumer<NewSensorDataEventConsumer>(context);
                    });
                });
            });

            builder.Services.AddScoped<ISensorBroadcaster, SignalRSensorBroadcaster>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSerilogRequestLogging();
            app.UseCors("AllowFrontend");
            app.MapHub<SensorHub>("/hubs/sensors");

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
