import { lazy } from 'react';
import type { Route } from './types';

const Home = lazy(() => import('@pages/home'));

export const ROUTES: Route[] = [
  {
    path: '/',
    content: Home,
  },
];
