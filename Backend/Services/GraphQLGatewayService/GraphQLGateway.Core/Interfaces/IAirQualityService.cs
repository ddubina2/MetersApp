using GraphQLGateway.Core.Dto;

namespace GraphQLGateway.Core.Interfaces;

public interface IAirQualityService
{
    IQueryable<AirQualityReadingDto> GetReadings();
}
