using GraphQLServer.Core.Dto;
using GraphQLServer.Core.Interfaces;
using GraphQLServer.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.Core.Services;

public class AirQualityService : IAirQualityService
{
    private readonly AppDbContext _dbContext;

    public AirQualityService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<AirQualityReadingDto> GetReadings()
    {
        return _dbContext.AirQualityReadings
            .AsNoTracking()
            .Select(x => new AirQualityReadingDto
            {
                Id = x.Id,
                LocationId = x.LocationId,
                Timestamp = x.Timestamp,
                Co2 = x.Co2,
                Pm25 = x.Pm25,
                Humidity = x.Humidity,
            });
    }
}
