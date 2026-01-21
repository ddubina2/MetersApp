import { gql } from '@apollo/client';

export const GET_AIR_QUALITY_READINGS = gql`
  query GetAirQualityReadings($first: Int, $where: AirQualityReadingDtoFilterInput) {
    airQualityReadings(
      first: $first
      where: $where
      order: [{ timestamp: DESC }]
    ) {
      edges {
        node {
          id
          timestamp
          co2
          pm25
          humidity
          locationId
        }
      }
    }
  }
`;
