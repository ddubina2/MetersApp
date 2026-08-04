import { Toggle } from '@components/Toggle';
import { Typography } from '@components/Typography';
import { useTheme } from '@hooks/useTheme';
import { twMerge } from 'tailwind-merge';
import type { FC } from 'react';

export type HeaderProps = {
  className?: string;
};

export const Header: FC<HeaderProps> = ({ className }) => {
  const { theme, toggleTheme } = useTheme();
  const isDark = theme === 'dark';

  return (
    <header
      className={twMerge(
        'flex items-center justify-between px-5 py-4',
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
      <Toggle
        checked={isDark}
        onChange={toggleTheme}
        label={isDark ? 'Dark' : 'Light'}
      />
    </header>
  );
};
