import { createContext } from 'react';

export type Theme = 'light' | 'dark';

export type ThemePreference = 'system' | Theme;

export type ThemeContextValue = {
  theme: Theme;
  setTheme: (preference: ThemePreference) => void;
  toggleTheme: () => void;
};

export const ThemeContext = createContext<ThemeContextValue | null>(null);
