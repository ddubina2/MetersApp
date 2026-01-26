namespace GraphQLGateway.Core.Interfaces;

using GraphQLGateway.Core.Dto;

public interface IAirQualityService
{
    IQueryable<AirQualityReadingDto> GetReadings();
}
