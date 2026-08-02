using DataProcessor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataProcessor.Data;

public class DataProcessorDbContext : DbContext
{
    public DbSet<Location> Locations { get; set; }

    public DbSet<AirQualityReading> AirQualityReadings { get; set; }

    public DbSet<EnergyReading> EnergyReadings { get; set; }

    public DbSet<MotionReading> MotionReadings { get; set; }

    public DataProcessorDbContext(DbContextOptions<DataProcessorDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataProcessorDbContext).Assembly);
    }
}
