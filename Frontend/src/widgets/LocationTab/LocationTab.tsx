import { useQuery } from '@apollo/client/react';
import { Typography } from '@components/Typography';
import type { AirQualityReadingDto, EnergyReadingDto, GetAirQualityReadingsQuery, GetAirQualityReadingsQueryVariables, GetEnergyReadingsQuery, GetEnergyReadingsQueryVariables, GetMotionReadingsQuery, GetMotionReadingsQueryVariables, LocationType, MotionReadingDto } from '@shared/graphql/__generated__/graphql';
import { GET_AIR_QUALITY_READINGS } from '@shared/graphql/queries/getAirQuality';
import { GET_ENERGY_READINGS } from '@shared/graphql/queries/getEnergy';
import { GET_MOTION_READINGS } from '@shared/graphql/queries/getMotion';
import { SensorType } from '@shared/hooks/useSensorsHub';
import { toChartTime } from '@shared/utils/formatDateTime';
import { ReadingsChart } from '@widgets/ReadingsChart';
import { useMemo, type FC } from 'react';

type LocationTabProps = {
  type: LocationType
}

const ITEMS_COUNT = 100;

export const LocationTab: FC<LocationTabProps> = ({ type }) => {

  const initialTimestamp = useMemo(
    () => new Date(Date.now() - 300_000).toISOString(),
    []
  );

  const { data: airQualityData, loading: airQualityLoading, error: airQualityError } = useQuery<GetAirQualityReadingsQuery, GetAirQualityReadingsQueryVariables>(
    GET_AIR_QUALITY_READINGS,
    { variables: {
      first: ITEMS_COUNT,
      where: {
        timestamp: { gte: initialTimestamp },
        locationId: { eq: type },
     }
    },
    fetchPolicy: 'no-cache',
   }
  );

  const { data: motionData, loading: motionLoading, error: motionError } = useQuery<GetMotionReadingsQuery, GetMotionReadingsQueryVariables>(
    GET_MOTION_READINGS,
    { variables: {
      first: ITEMS_COUNT,
      where: {
        timestamp: { gte: initialTimestamp },
        locationId: { eq: type },
     }
    },
    fetchPolicy: 'no-cache',
   }
  );

  const { data: energyData, loading: energyLoading, error: energyError } = useQuery<GetEnergyReadingsQuery, GetEnergyReadingsQueryVariables>(
    GET_ENERGY_READINGS,
    { variables: {
      first: ITEMS_COUNT,
      where: {
        timestamp: { gte: initialTimestamp },
        locationId: { eq: type },
     },
    },
    fetchPolicy: 'no-cache',
   }
  );

  return (
    <div className='mt-4 flex flex-col items-center justify-center'>
      <Typography text='Air Quality' />
      <ReadingsChart<AirQualityReadingDto>
        locationType={type}
        sensorType={SensorType.AirQuality}
        initialData={airQualityData?.airQualityReadings?.edges?.map(e => e.node) || []}
        isLoading={airQualityLoading}
        isError={!!airQualityError}
        mapToChart={(node) => ({
                name: toChartTime(node.timestamp),
                co2: node.co2,
                pm25: node.pm25,
                humidity: node.humidity,
            })}
      />

      <Typography text='Motion' />
      <ReadingsChart<MotionReadingDto>
        locationType={type}
        sensorType={SensorType.Motion}
        withGradient
        initialData={motionData?.motionReadings?.edges?.map(e => e.node) || []}
        isLoading={motionLoading}
        isError={!!motionError}
        mapToChart={(node) => ({
                name: toChartTime(node.timestamp),
                motionDetected: node.motionDetected ? 1 : 0,
            })}
      />

      <Typography text='Energy' />
      <ReadingsChart<EnergyReadingDto>
        locationType={type}
        sensorType={SensorType.Energy}
        withGradient
        initialData={energyData?.energyReadings?.edges?.map(e => e.node) || []}
        isLoading={energyLoading}
        isError={!!energyError}
        mapToChart={(node) => ({
                name: toChartTime(node.timestamp),
                energy: node.energy.toFixed(2),
            })}
      />
    </div>
  );
};
