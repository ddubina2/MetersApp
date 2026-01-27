import { twMerge } from 'tailwind-merge';
import { type FC } from 'react';
import { styles } from './Button.styles';
import type { ButtonProps } from './types';

/**
 * Primary component for any kind of button on page.
 * Allows you to insert text or JSX elements.
 * @example
 * ```tsx
 * // With text
 * <Button text="login" />
 * // With JSX
 * <Button><div /></Button>
 * ```
 */
export const Button: FC<ButtonProps> = ({
  children,
  onClick = () => null,
  text,
  className,
  intent = 'primary',
  ...props
}) => {

  return (
    <button
      disabled={props.disabled}
      onClick={onClick}
      className={twMerge(styles({ intent }), className)}
      {...props}
    >
      {text ?? children}
    </button>
  );
};
