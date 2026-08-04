import { useCallback, useEffect, useRef, useState } from 'react';
import { envs } from '@shared/envs';

export type CleaningResult = 'NotPerformed' | 'Success' | 'Failure';

export type CleanupStatus = {
  timeRemaining: string;
  lastResult: CleaningResult;
  isLoading: boolean;
};

const baseUrl = envs.VITE_API_BASE_URL.replace('/graphql', '');

const TICK_INTERVAL = 1000;
const REFRESH_DELAY = 5000;

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

  const targetTimeRef = useRef<number>(0);
  const intervalRef = useRef<ReturnType<typeof setInterval> | null>(null);
  const timeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const fetchAndStartRef = useRef<() => Promise<void>>(async () => {});

  const clearTimers = useCallback(() => {
    if (intervalRef.current) {
      clearInterval(intervalRef.current);
      intervalRef.current = null;
    }
    if (timeoutRef.current) {
      clearTimeout(timeoutRef.current);
      timeoutRef.current = null;
    }
  }, []);

  const startTick = useCallback(() => {
    clearTimers();

    intervalRef.current = setInterval(() => {
      const remaining = targetTimeRef.current - Date.now();

      if (remaining <= 0) {
        clearTimers();
        setTimeRemaining('00:00:00');

        timeoutRef.current = setTimeout(() => {
          fetchAndStartRef.current();
        }, REFRESH_DELAY);
      } else {
        setTimeRemaining(formatDuration(remaining));
      }
    }, TICK_INTERVAL);
  }, [clearTimers]);

  fetchAndStartRef.current = async () => {
    try {
      const response = await fetch(`${baseUrl}/api/sensor-data/next-cleanup`);
      if (!response.ok)
        throw new Error('Failed to fetch cleanup status');

      const data = (await response.json()) as ApiResponse;
      targetTimeRef.current = new Date(data.nextCleanup).getTime();

      setLastResult(parseResult(data.lastCleaningResult));
      setTimeRemaining(formatDuration(targetTimeRef.current - Date.now()));
    } catch {
      targetTimeRef.current = 0;
      setTimeRemaining('00:00:00');
      setLastResult('NotPerformed');
    } finally {
      setIsLoading(false);
    }

    startTick();
  };

  useEffect(() => {
    fetchAndStartRef.current();

    return () => {
      clearTimers();
    };
  }, [clearTimers]);

  return { timeRemaining, lastResult, isLoading };
};
