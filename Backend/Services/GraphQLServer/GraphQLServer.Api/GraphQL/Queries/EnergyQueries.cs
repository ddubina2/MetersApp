using GraphQLServer.Core.Dto;
using GraphQLServer.Core.Interfaces;

namespace GraphQLServer.Api.GraphQL.Queries;

[ExtendObjectType("Query")]
public class EnergyQueries
{
    [UsePaging(IncludeTotalCount = true, MaxPageSize = 100)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EnergyReadingDto> EnergyReadings(
        [Service] IEnergyService service)
        => service.GetReadings();
}
