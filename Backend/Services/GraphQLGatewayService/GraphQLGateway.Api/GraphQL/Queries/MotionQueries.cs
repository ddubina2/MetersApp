using GraphQLGateway.Core.Dto;
using GraphQLGateway.Core.Interfaces;

namespace GraphQLGateway.Api.GraphQL.Queries;

[ExtendObjectType("MotionReadings")]
public class MotionQueries
{
    [UsePaging(IncludeTotalCount = true, MaxPageSize = 100)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MotionReadingDto> MotionReadings(
        [Service] IMotionService service)
        => service.GetReadings();
}
