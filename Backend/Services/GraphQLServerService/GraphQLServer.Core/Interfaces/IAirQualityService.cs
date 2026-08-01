using GraphQLServer.Core.Dto;

namespace GraphQLServer.Core.Interfaces;

public interface IAirQualityService
{
    IQueryable<AirQualityReadingDto> GetReadings();
}
