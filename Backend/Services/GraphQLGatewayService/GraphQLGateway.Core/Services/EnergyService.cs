namespace GraphQLGateway.Core.Services;

using GraphQLGateway.Core.Dto;
using GraphQLGateway.Core.Interfaces;
using GraphQLGateway.Data;
using Microsoft.EntityFrameworkCore;

public class EnergyService : IEnergyService
{
    private readonly AppDbContext _db;

    public EnergyService(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<EnergyReadingDto> GetReadings()
    {
        return _db.EnergyReadings
            .AsNoTracking()
            .Select(x => new EnergyReadingDto
            {
                Id = x.Id,
                LocationId = x.LocationId,
                Timestamp = x.Timestamp,
                Energy = x.Energy,
            });
    }
}
