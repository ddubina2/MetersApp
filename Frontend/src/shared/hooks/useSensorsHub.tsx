/* eslint-disable @typescript-eslint/no-explicit-any */
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { envs } from '@shared/envs';
import { useEffect } from 'react';

export type SensorData = {
  items: SensorDataItem[];
}

type SensorDataItem = {
  sensorType: SensorType;
  locationType: string;
  timestamp: string;

  payload: any;
}

export enum SensorType {
  AirQuality = 'AirQuality',
  Motion = 'Motion',
  Energy = 'Energy',
}

type UseSensorsHubParams = {
  onNewDataAvailable: (data: SensorData) => void;
}

export const useSensorsHub = ({ onNewDataAvailable }: UseSensorsHubParams) => {
  useEffect(() => {
    const hub = new HubConnectionBuilder()
      .withUrl(envs.VITE_SENSORS_HUB_URL)
      .configureLogging(LogLevel.Warning)
      .withAutomaticReconnect()
      .build();

    hub.start()
      .then(() => {})
      .catch(console.error);

    hub.on('ReceiveSensorData', (reading: SensorData) => {
      onNewDataAvailable(reading);
    });

    return () => {
      hub.stop();
    };
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return;
};
