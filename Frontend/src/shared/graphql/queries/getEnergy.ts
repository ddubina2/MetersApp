import { gql } from '@apollo/client';

export const GET_ENERGY_READINGS = gql`
  query GetEnergyReadings($first: Int, $where: EnergyReadingDtoFilterInput) {
    energyReadings(
      first: $first
      where: $where
      order: [{ timestamp: DESC }]
    ) {
      edges {
        node {
          id
          timestamp
          energy
          locationId
        }
      }
    }
  }
`;
