import { useCallback, useEffect, useRef, useState } from 'react';
import { envs } from '@shared/envs';

export type CleaningResult = 'NotPerformed' | 'Success' | 'Failure';

export type CleanupStatus = {
  timeRemaining: string;
  lastResult: CleaningResult;
  isLoading: boolean;
};

const baseUrl = envs.VITE_API_BASE_URL.replace('/graphql', '');

const POLL_INTERVAL = 10_000;

const parseResult = (value: number): CleaningResult => {
  switch (value) {
    case 2:
      return 'Success';
    case 3:
      return 'Failure';
    default:
      return 'NotPerformed';
  }
};

const formatDuration = (ms: number): string => {
  const totalSeconds = Math.max(0, Math.floor(ms / 1000));
  const hours = Math.floor(totalSeconds / 3600);
  const minutes = Math.floor((totalSeconds % 3600) / 60);
  const seconds = totalSeconds % 60;

  const pad = (n: number) => String(n).padStart(2, '0');

  return `${pad(hours)}:${pad(minutes)}:${pad(seconds)}`;
};

type ApiResponse = {
  lastCleaningResult: number;
  nextCleanup: string;
};

export const useCleanupStatus = (): CleanupStatus => {
  const [timeRemaining, setTimeRemaining] = useState('00:00:00');
  const [lastResult, setLastResult] = useState<CleaningResult>('NotPerformed');
  const [isLoading, setIsLoading] = useState(true);
  const intervalRef = useRef<ReturnType<typeof setInterval> | null>(null);

  const fetchStatus = useCallback(async () => {
    try {
      const response = await fetch(`${baseUrl}/api/sensor-data/next-cleanup`);
      if (!response.ok)
        throw new Error('Failed to fetch cleanup status');

      const data = (await response.json()) as ApiResponse;
      const nextDate = new Date(data.nextCleanup).getTime();
      const remaining = nextDate - Date.now();

      setTimeRemaining(formatDuration(remaining));
      setLastResult(parseResult(data.lastCleaningResult));
    } catch {
      setTimeRemaining('00:00:00');
      setLastResult('NotPerformed');
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchStatus();
    intervalRef.current = setInterval(fetchStatus, POLL_INTERVAL);

    return () => {
      if (intervalRef.current)
        clearInterval(intervalRef.current);
    };
  }, [fetchStatus]);

  return { timeRemaining, lastResult, isLoading };
};
