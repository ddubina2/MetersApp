using GraphQLGateway.Core.Dto;
using GraphQLGateway.Core.Interfaces;
using GraphQLGateway.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQLGateway.Core.Services;

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
            .Select(x => new EnergyReadingDto(
                x.Id,
                x.LocationId,
                x.Timestamp,
                x.Energy
            ));
    }

    public IQueryable<EnergyAggregationDto> GetEnergyByDay()
    {
        return _db.EnergyReadings
            .AsNoTracking()
            .GroupBy(x => new
            {
                x.LocationId,
                Day = x.Timestamp.Date
            })
            .Select(g => new EnergyAggregationDto(
                g.Key.LocationId,
                g.Key.Day,
                g.Sum(x => x.Energy)
            ));
    }
}

