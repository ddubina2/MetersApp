using GraphQLGateway.Core.Dto;

namespace GraphQLGateway.Core.Interfaces;

public interface IEnergyService
{
    IQueryable<EnergyReadingDto> GetReadings();
}
