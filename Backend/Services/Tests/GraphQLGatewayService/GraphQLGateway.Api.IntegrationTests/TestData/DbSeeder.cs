using GraphQLGateway.Data;
using GraphQLGateway.Data.Entities;

namespace GraphQLGateway.Api.IntegrationTests.TestData;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        db.AirQualityReadings.AddRange(
            new AirQualityReading
            {
                Id = Guid.NewGuid(),
                Co2 = 100,
                Pm25 = 50,
                Humidity = 25,
                Timestamp = DateTime.UtcNow,
            },
            new AirQualityReading
            {
                Id = Guid.NewGuid(),
                Co2 = 25,
                Pm25 = 55,
                Humidity = 100,
                Timestamp = DateTime.UtcNow,
            });

        db.SaveChanges();
    }
}
