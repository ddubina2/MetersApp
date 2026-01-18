using DataIngestor.Core;
using MassTransit;
using MetersApp.Shared.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));
builder.Services.AddMassTransit(x =>
{
    var options = builder.Configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();
    x.UsingRabbitMq((context,cfg) =>
    {
        cfg.Host(options?.Host, "/", h => {
            h.Username(options?.UserName ?? "");
            h.Password(options?.Password ?? "");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.ConfigureCoreServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
