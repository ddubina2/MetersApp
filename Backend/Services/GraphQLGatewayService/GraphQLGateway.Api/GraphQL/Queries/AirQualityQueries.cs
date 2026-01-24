using GraphQLGateway.Core.Dto;
using GraphQLGateway.Core.Interfaces;

namespace GraphQLGateway.Api.GraphQL.Queries;

[ExtendObjectType("AirQualityReadings")]
public class AirQualityQueries
{
    [UsePaging(IncludeTotalCount = true, MaxPageSize = 100)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AirQualityReadingDto> AirQualityReadings([Service] IAirQualityService service)
        => service.GetReadings();
}
