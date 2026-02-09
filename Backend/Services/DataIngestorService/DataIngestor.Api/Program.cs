using DataIngestor.Core;
using MassTransit;
using MetersApp.Shared.Extensions;
using MetersApp.Shared.Middlewares;
using MetersApp.Shared.Options;
using Serilog;
using Serilog.Events;

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
        "data-ingestor",
        "DataIngestor.Metrics");

    builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));
    builder.Services.AddMassTransit(x =>
    {
        var options = builder.Configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(options?.Host, "/", h =>
            {
                h.Username(options?.UserName ?? string.Empty);
                h.Password(options?.Password ?? string.Empty);
            });

            cfg.ConfigureEndpoints(context);
        });
    });

    builder.Services.ConfigureCoreServices(builder.Configuration);

    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseSerilogRequestLogging();

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
