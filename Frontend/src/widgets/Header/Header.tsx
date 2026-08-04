import { Toggle } from '@components/Toggle';
import { Typography } from '@components/Typography';
import { useTheme } from '@hooks/useTheme';
import { useCleanupStatus } from '@hooks/useCleanupStatus';
import { twMerge } from 'tailwind-merge';
import type { FC } from 'react';

const resultColorClass: Record<string, string> = {
  Success: 'bg-green-500',
  Failure: 'bg-red-500',
  NotPerformed: 'bg-gray-400',
};

export type HeaderProps = {
  className?: string;
};

export const Header: FC<HeaderProps> = ({ className }) => {
  const { theme, toggleTheme } = useTheme();
  const { timeRemaining, lastResult, isLoading } = useCleanupStatus();
  const isDark = theme === 'dark';

  return (
    <header
      className={twMerge(
        'flex items-center justify-between px-5 py-2',
        'bg-raised border-b border-line',
        className
      )}
    >
      <Typography
        tag='h1'
        text='Meters App'
        weight='bold'
        className='text-xl'
      />
      <div className='flex items-center gap-6'>
        <div className='flex items-center gap-2'>
          <span className='select-none text-sm font-medium text-secondary'>
            Next cleanup:
          </span>
          <span className='select-none text-sm font-bold tabular-nums text-regular'>
            {isLoading ? '...' : timeRemaining}
          </span>
          <span
            className={twMerge(
              'inline-block size-2 rounded-full',
              resultColorClass[lastResult]
            )}
          />
          <span className='select-none text-xs text-secondary'>
            {lastResult}
          </span>
        </div>
        <div className='flex items-center gap-2'>
          <span className='select-none text-sm font-medium text-regular'>
            {isDark ? 'Dark' : 'Light'}
          </span>
          <Toggle
            checked={isDark}
            onChange={toggleTheme}
          />
        </div>
      </div>
    </header>
  );
};
