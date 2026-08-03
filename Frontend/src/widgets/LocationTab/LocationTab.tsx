import type { WatchQueryFetchPolicy } from '@apollo/client';
import { useQuery } from '@apollo/client/react';
import { Button } from '@components/Button';
import { Typography } from '@components/Typography';
import type { AirQualityReadingDto, EnergyReadingDto, GetAirQualityReadingsQuery, GetAirQualityReadingsQueryVariables, GetEnergyReadingsQuery, GetEnergyReadingsQueryVariables, GetMotionReadingsQuery, GetMotionReadingsQueryVariables, LocationType, MotionReadingDto } from '@shared/graphql/__generated__/graphql';
import { GET_AIR_QUALITY_READINGS } from '@shared/graphql/queries/getAirQuality';
import { GET_ENERGY_READINGS } from '@shared/graphql/queries/getEnergy';
import { GET_MOTION_READINGS } from '@shared/graphql/queries/getMotion';
import { SensorType } from '@hooks/useSensorsHub';
import { toChartTime } from '@shared/utils/formatDateTime';
import { ReadingsChart } from '@widgets/ReadingsChart';
import { useMemo, useState, type FC } from 'react';

type LocationTabProps = {
  type: LocationType
}

const RECORD_MAX_COUNT = 100;
const FETCH_POLICY: WatchQueryFetchPolicy = 'no-cache';
const TIME_FILTERS = [1, 5, 10]; // in minutes

export const LocationTab: FC<LocationTabProps> = ({ type }) => {

  const [currentTimeFilter, setCurrentTimeFilter] = useState(5); // in minutes
  const currentTimeStamp = useMemo(
    () => new Date(Date.now() - currentTimeFilter * 60_000).toISOString(),
    [currentTimeFilter]
  );

  const { data: airQualityData, loading: airQualityLoading, error: airQualityError } = useQuery<GetAirQualityReadingsQuery, GetAirQualityReadingsQueryVariables>(
    GET_AIR_QUALITY_READINGS,
    { variables: {
      first: RECORD_MAX_COUNT,
      where: {
        timestamp: { gte: currentTimeStamp },
        locationId: { eq: type },
     }
    },
    fetchPolicy: FETCH_POLICY,
   }
  );

  const { data: motionData, loading: motionLoading, error: motionError } = useQuery<GetMotionReadingsQuery, GetMotionReadingsQueryVariables>(
    GET_MOTION_READINGS,
    { variables: {
      first: RECORD_MAX_COUNT,
      where: {
        timestamp: { gte: currentTimeStamp },
        locationId: { eq: type },
     }
    },
    fetchPolicy: FETCH_POLICY,
   }
  );

  const { data: energyData, loading: energyLoading, error: energyError } = useQuery<GetEnergyReadingsQuery, GetEnergyReadingsQueryVariables>(
    GET_ENERGY_READINGS,
    { variables: {
      first: RECORD_MAX_COUNT,
      where: {
        timestamp: { gte: currentTimeStamp },
        locationId: { eq: type },
     },
    },
    fetchPolicy: FETCH_POLICY,
   }
  );

  return (
    <div className='mt-4 flex flex-col items-center justify-center'>
      <div className='flex gap-2'>
        {TIME_FILTERS.map((value) =>(
          <Button
            key={`time-filter-${value}`}
            text={`${value} Min`}
            intent={currentTimeFilter === value ? 'primary' : 'secondary'}
            onClick={() => setCurrentTimeFilter(value)}
            className='py-0'
          />
        ))}
      </div>
      <Typography text='Air Quality' weight='bold' className='mt-2' />
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

      <Typography text='Motion' weight='bold' />
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

      <Typography text='Energy' weight='bold' />
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
