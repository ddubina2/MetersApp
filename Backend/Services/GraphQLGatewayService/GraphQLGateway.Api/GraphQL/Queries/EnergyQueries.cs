namespace GraphQLGateway.Api.GraphQL.Queries;

using GraphQLGateway.Core.Dto;
using GraphQLGateway.Core.Interfaces;

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
