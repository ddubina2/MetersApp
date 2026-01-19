using MassTransit;
using MetersApp.Shared.Options;
using Notifications.Api.SignalR;
using Notifications.Core.Consumers;
using Notifications.Core.Interfaces;

namespace Notifications.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSignalR();
        builder.Services.AddScoped<ISensorBroadcaster, SignalRSensorBroadcaster>();
        builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));
        builder.Services.AddMassTransit(x =>
        {
            var options = builder.Configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();

            x.AddConsumer<NewSensorDataEventConsumer>();
            x.UsingRabbitMq((context,cfg) =>
            {
                cfg.Host(options?.Host, "/", h => {
                    h.Username(options?.UserName ?? "");
                    h.Password(options?.Password ?? "");
                });

                cfg.ReceiveEndpoint(e =>
                {
                    e.ConfigureConsumer<NewSensorDataEventConsumer>(context);
                });
            });
        });

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.Run();
    }
}
