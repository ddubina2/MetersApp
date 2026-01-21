import type { ComponentType } from 'react';

export type RoutePath =
  '/'

type BaseRoute = {
  path: RoutePath;
  content: ComponentType;
};

export type Route = BaseRoute
