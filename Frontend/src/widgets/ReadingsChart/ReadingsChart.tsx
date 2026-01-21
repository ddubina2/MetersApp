import { LineChart } from '@components/LineChart';
import type { LineChartData } from '@components/LineChart/types';
import { LocationType } from '@shared/graphql/__generated__/graphql';
import type { SensorData } from '@shared/hooks/useSensorsHub';
import { SensorType } from '@shared/hooks/useSensorsHub';
import { useSensorsHub } from '@shared/hooks/useSensorsHub';
import { toChartTime } from '@shared/utils/formatDateTime';
import { Loadable } from '@widgets/Loadable/Loadable';
import { useEffect, useState } from 'react';

type ReadingsChartProps<T> = {
  locationType: LocationType;
  sensorType: SensorType
  isLoading: boolean;
  isError?: boolean;
  className?: string;
  withGradient?: boolean;
  initialData?: T[];
};

export const ReadingsChart = <T, >({
  locationType,
  sensorType,
  initialData,
  isLoading,
  isError,
  mapToChart,
  withGradient,
}: ReadingsChartProps<T> & { mapToChart: (reading: T) => LineChartData }) => {
  const [liveData, setLiveData] = useState<LineChartData[]>([]);
  const [isAnimationActive, setIsAnimationActive] = useState(true);

  useEffect(() => {
    if (!initialData) return;

    const chartData = initialData.map(mapToChart);
    setIsAnimationActive(true);
    setLiveData(chartData);
  }, [initialData, mapToChart]);

  const onNewDataAvailable = (data: SensorData) => {
    const relevantReadings = data.items.filter(item => {
      const itemKey = Object.keys(LocationType).find(
        k => LocationType[k as keyof typeof LocationType] === locationType
      );
      return itemKey === item.locationType && item.sensorType === sensorType;
    });

    if (relevantReadings.length === 0) return;

    setIsAnimationActive(false);

    setLiveData(prevData => {

      const newChartData: LineChartData[] = relevantReadings.map(item => {
        switch (item.sensorType) {
          case SensorType.AirQuality:
            return {
              name: toChartTime(item.timestamp),
              co2: item.payload.co2,
              humidity: item.payload.humidity,
              pm25: item.payload.pm25,
            };
          case SensorType.Motion:
            return {
              name: toChartTime(item.timestamp),
              motionDetected: item.payload.motionDetected ? 1 : 0,
            };
          case SensorType.Energy:
            return {
              name: toChartTime(item.timestamp),
              energy: item.payload.energy.toFixed(2),
            };
        }
      });

      // Append new readings
      return [...newChartData, ...prevData];
    });
  };

  useSensorsHub({ onNewDataAvailable });

  return (<>
    <Loadable isLoading={isLoading} error={isError} containerClassName='min-h-[234px] min-w-[200px]'>
      <LineChart
        withGradient={withGradient}
        className='w-full'
        dataset={[...liveData].reverse()}
        isAnimationActive={isAnimationActive}
      />
    </Loadable>
  </>);
};

