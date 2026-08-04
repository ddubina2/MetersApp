import { Header } from '@widgets/Header';
import type { FC, PropsWithChildren } from 'react';

export const Layout: FC<PropsWithChildren> = ({ children }) => {

  return (
    <div className='flex min-h-dvh flex-col bg-surface'>
      <Header />
      <main className='flex-1 p-5'>
        {children}
      </main>
    </div>
  );
};
