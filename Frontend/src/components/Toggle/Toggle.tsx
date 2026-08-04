import { twMerge } from 'tailwind-merge';
import { type FC, type ChangeEvent } from 'react';

export type ToggleProps = {
  checked: boolean;
  onChange: (checked: boolean) => void;
  label?: string;
  className?: string;
};

export const Toggle: FC<ToggleProps> = ({ checked, onChange, label, className }) => {
  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    onChange(e.target.checked);
  };

  return (
    <label
      className={twMerge(
        'inline-flex cursor-pointer items-center gap-2',
        className
      )}
    >
      <span className='select-none text-sm font-medium text-regular'>
        {label}
      </span>
      <div className='relative inline-flex items-center'>
        <input
          type='checkbox'
          className='peer sr-only'
          checked={checked}
          onChange={handleChange}
        />
        <div
          className={twMerge(
            'h-6 w-11 rounded-full border-2 transition-colors duration-200 ease-in-out',
            'border-line peer-checked:border-primary peer-checked:bg-primary',
            'bg-hover'
          )}
        />
        <div
          className={twMerge(
            'absolute top-1 left-1 h-4 w-4 rounded-full bg-on-primary transition-transform duration-200 ease-in-out',
            checked ? 'translate-x-5' : 'translate-x-0'
          )}
        />
      </div>
    </label>
  );
};
