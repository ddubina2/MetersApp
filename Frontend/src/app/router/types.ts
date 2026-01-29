import type { ComponentType } from 'react';

export type RoutePath =
  '/'

export type BaseRoute = {
  path: RoutePath;
  content: ComponentType;
};
