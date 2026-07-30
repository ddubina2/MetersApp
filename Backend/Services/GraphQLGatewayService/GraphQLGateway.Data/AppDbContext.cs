using GraphQLGateway.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraphQLGateway.Data;

public class AppDbContext : DbContext
{
    public DbSet<Location> Locations => Set<Location>();

    public DbSet<AirQualityReading> AirQualityReadings => Set<AirQualityReading>();

    public DbSet<EnergyReading> EnergyReadings => Set<EnergyReading>();

    public DbSet<MotionReading> MotionReadings => Set<MotionReading>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
