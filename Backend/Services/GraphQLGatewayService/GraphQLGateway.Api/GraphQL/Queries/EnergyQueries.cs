using GraphQLGateway.Core.Dto;
using GraphQLGateway.Core.Interfaces;

namespace GraphQLGateway.Api.GraphQL.Queries;

[ExtendObjectType(Name = "Query")]
public class EnergyQueries
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EnergyReadingDto> EnergyReadings(
        [Service] IEnergyService service)
        => service.GetReadings();

    [UsePaging]
    public IQueryable<EnergyAggregationDto> EnergyByDay(
        [Service] IEnergyService service)
        => service.GetEnergyByDay();
}
