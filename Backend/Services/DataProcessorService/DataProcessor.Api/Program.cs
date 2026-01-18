using DataProcessor.Core.Consumers;
using DataProcessor.Data;
using MassTransit;
using MetersApp.Shared.Constants;
using MetersApp.Shared.Options;
using Microsoft.EntityFrameworkCore;

namespace DataProcessor.Api;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<DataProcessorDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));
        builder.Services.AddMassTransit(x =>
        {
            var options = builder.Configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();

            x.AddConsumer<SensorDataBatchConsumer>();

            x.UsingRabbitMq((context,cfg) =>
            {
                cfg.Host(options?.Host, "/", h => {
                    h.Username(options?.UserName ?? "");
                    h.Password(options?.Password ?? "");
                });

                cfg.ReceiveEndpoint(QueueNames.ProcessSensorDataQueue, e =>
                {
                    e.ConfigureConsumer<SensorDataBatchConsumer>(context);
                });
            });
        });

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
