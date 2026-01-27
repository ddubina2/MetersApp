namespace GraphQLGateway.Api.GraphQL.Types;

using GraphQLGateway.Core.Dto;

public class AirQualityReadingType : ObjectType<AirQualityReadingDto>
{
    protected override void Configure(IObjectTypeDescriptor<AirQualityReadingDto> descriptor)
    {
        descriptor.Field(x => x.Id).Type<NonNullType<UuidType>>();
        descriptor.Field(x => x.LocationId).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.Timestamp).Type<NonNullType<DateTimeType>>();
        descriptor.Field(x => x.Co2).Type<NonNullType<IntType>>();
        descriptor.Field(x => x.Pm25).Type<NonNullType<IntType>>();
        descriptor.Field(x => x.Humidity).Type<NonNullType<IntType>>();
    }
}
