import { lazy } from 'react';
import type { BaseRoute } from './types';

const Home = lazy(() => import('@pages/home'));

export const ROUTES: BaseRoute[] = [
  {
    path: '/',
    content: Home,
  },
];
