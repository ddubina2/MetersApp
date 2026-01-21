import { cva } from 'class-variance-authority';
import { type FC, createElement } from 'react';
import { twMerge } from 'tailwind-merge';
import type { TypographyProps } from './types';

const styles = cva('text-inherit', {
  variants: {
    weight: {
      regular: 'font-normal',
      bold: 'font-bold',
      inherit: 'font-inherit',
    },
    color: {
      regular: 'text-regular',
      secondary: 'text-secondary'
    }
  },
});

/**
 * Primary component for any kind of typography on page.
 * @example
 * ```tsx
 * <Typography text="login" />
 * ```
 */
export const Typography: FC<TypographyProps> = ({
  id,
  text,
  tag = 'p',
  weight = 'regular',
  color = 'regular',
  className,
  ...rest
}) => {

  return createElement(
    tag,
    {
      'id': id,
      'className': twMerge(styles({ weight, color }),
       'font-sans',
        className),
      ...rest
    },
    text,
  );
};
