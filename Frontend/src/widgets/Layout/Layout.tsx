import type { FC, PropsWithChildren } from 'react';

export const Layout: FC<PropsWithChildren> = ({ children }) => {

  return (<div className='size-full dark:bg-black'>
    {children}
  </div>);
};
