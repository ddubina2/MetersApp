using Microsoft.EntityFrameworkCore;

namespace DataCleaner.Data;

public class DataCleanerDbContext : DbContext
{
    public DataCleanerDbContext(DbContextOptions<DataCleanerDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataCleanerDbContext).Assembly);
    }
}
