import type { FC, PropsWithChildren } from 'react';
import { twMerge } from 'tailwind-merge';
import { Icon } from '@components/Icon';
import { Typography } from '@components/Typography';

type LoadableProps = {
  isLoading: boolean;
  error?: boolean; // new prop to indicate error
  containerClassName?: string;
  iconClassName?: string;
};

export const Loadable: FC<PropsWithChildren<LoadableProps>> = ({
  children,
  isLoading,
  error = false,
  containerClassName,
  iconClassName,
}) => {
  if (isLoading || error) {
    const iconId = error ? 'warning' : 'loader';
    const iconClasses = twMerge(
      'size-6',
      error ? 'text-error' : 'animate-spin',
      iconClassName
    );

    return (
      <div
        className={twMerge(
          'flex w-full h-full items-center justify-center flex-col gap-2',
          containerClassName
        )}
      >
        <Icon id={iconId} className={iconClasses} />
        {error ? <Typography text='Error' className='text-error' /> : null}
      </div>
    );
  }

  return <>{children}</>;
};
