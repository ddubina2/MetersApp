import { twMerge } from 'tailwind-merge';
import { type FC } from 'react';

export type ToggleProps = {
  checked: boolean;
  onChange: (checked: boolean) => void;
  className?: string;
};

export const Toggle: FC<ToggleProps> = ({ checked, onChange, className }) => (
  <button
    type='button'
    role='switch'
    aria-checked={checked}
    onClick={() => onChange(!checked)}
    className={twMerge(
      'relative inline-flex h-6 w-11 cursor-pointer items-center rounded-full border-2 transition-colors duration-200 ease-in-out',
      'border-line bg-hover',
      checked && 'border-primary bg-primary',
      className
    )}
  >
    <span
      className={twMerge(
        'absolute top-1 left-1 h-4 w-4 rounded-full bg-on-primary transition-transform duration-200 ease-in-out',
        checked ? 'translate-x-5' : 'translate-x-0'
      )}
    />
  </button>
);
