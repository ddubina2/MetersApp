using DataCleaner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataCleaner.Data;

public class DataCleanerDbContext : DbContext
{
    public DbSet<Location> Locations { get; set; }

    public DbSet<AirQualityReading> AirQualityReadings { get; set; }

    public DbSet<EnergyReading> EnergyReadings { get; set; }

    public DbSet<MotionReading> MotionReadings { get; set; }

    public DataCleanerDbContext(DbContextOptions<DataCleanerDbContext> options)
        : base(options)
    {
    }
}
