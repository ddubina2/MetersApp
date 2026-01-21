import { gql } from '@apollo/client';

export const GET_MOTION_READINGS = gql`
  query GetMotionReadings($first: Int, $where: MotionReadingDtoFilterInput) {
    motionReadings(
      first: $first
      where: $where
      order: [{ timestamp: DESC }]
    ) {
      edges {
        node {
          id
          timestamp
          motionDetected
          locationId
        }
      }
    }
  }
`;
