namespace GraphQLGateway.Api.GraphQL.Queries;

using GraphQLGateway.Core.Dto;
using GraphQLGateway.Core.Interfaces;

[ExtendObjectType("Query")]
public class AirQualityQueries
{
    [UsePaging(IncludeTotalCount = true, MaxPageSize = 100)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AirQualityReadingDto> AirQualityReadings([Service] IAirQualityService service)
        => service.GetReadings();
}
