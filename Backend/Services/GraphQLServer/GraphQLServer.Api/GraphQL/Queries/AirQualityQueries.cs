using GraphQLServer.Core.Dto;
using GraphQLServer.Core.Interfaces;

namespace GraphQLServer.Api.GraphQL.Queries;

[ExtendObjectType("Query")]
public class AirQualityQueries
{
    [UsePaging(IncludeTotalCount = true, MaxPageSize = 100)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AirQualityReadingDto> AirQualityReadings([Service] IAirQualityService service)
        => service.GetReadings();
}
