using GraphQLGateway.Core.Dto;
using GraphQLGateway.Core.Interfaces;
using GraphQLGateway.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQLGateway.Core.Services;

public class AirQualityService : IAirQualityService
{
    private readonly AppDbContext _db;

    public AirQualityService(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<AirQualityReadingDto> GetReadings()
    {
        return _db.AirQualityReadings
            .AsNoTracking()
            .Select(x => new AirQualityReadingDto
            {
                Id = x.Id,
                LocationId = x.LocationId,
                Timestamp = x.Timestamp,
                Co2 = x.Co2,
                Pm25 = x.Pm25,
                Humidity = x.Humidity
            });
    }
}
