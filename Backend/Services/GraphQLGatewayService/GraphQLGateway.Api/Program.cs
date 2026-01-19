using GraphQLGateway.Api.GraphQL.Queries;
using GraphQLGateway.Api.GraphQL.Types;
using GraphQLGateway.Core;
using GraphQLGateway.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQLGateway.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddGraphQLServer()
            .AddQueryType(d => d.Name("Query"))
            .AddType<AirQualityReadingType>()
            .AddTypeExtension<AirQualityQueries>()
            .AddTypeExtension<EnergyQueries>()
            .AddTypeExtension<MotionQueries>()
            .AddFiltering()
            .AddSorting()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);;

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
        // Add services to the container.
        builder.Services.ConfigureCoreServices(builder.Configuration);

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.MapGraphQL();
        app.Run();
    }
}
