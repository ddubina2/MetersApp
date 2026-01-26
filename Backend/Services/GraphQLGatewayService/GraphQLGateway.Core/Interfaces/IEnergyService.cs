namespace GraphQLGateway.Core.Interfaces;

using GraphQLGateway.Core.Dto;

public interface IEnergyService
{
    IQueryable<EnergyReadingDto> GetReadings();
}
