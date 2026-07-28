using GraphQLGateway.Api.IntegrationTests.TestData;
using GraphQLGateway.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQLGateway.Api.IntegrationTests.Infrastructure;

public class GraphqlWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.First(
                d => d.ServiceType ==
                     typeof(IDbContextOptionsConfiguration<AppDbContext>));
            services.Remove(dbContextDescriptor);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("GraphqlTestsDb");
            });

            var provider = services.BuildServiceProvider();

            using var scope = provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            DbSeeder.Seed(db);
        });
    }
}
