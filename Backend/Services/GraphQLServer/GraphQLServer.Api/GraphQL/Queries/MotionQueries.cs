using GraphQLServer.Core.Dto;
using GraphQLServer.Core.Interfaces;

namespace GraphQLServer.Api.GraphQL.Queries;

[ExtendObjectType("Query")]
public class MotionQueries
{
    [UsePaging(IncludeTotalCount = true, MaxPageSize = 100)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MotionReadingDto> MotionReadings(
        [Service] IMotionService service)
        => service.GetReadings();
}
