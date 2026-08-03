import { createContext, useContext } from 'react';

export type Theme = 'light' | 'dark';

export type ThemePreference = 'system' | Theme;

type ThemeContextValue = {
  theme: Theme;
  setTheme: (preference: ThemePreference) => void;
  toggleTheme: () => void;
};

export const ThemeContext = createContext<ThemeContextValue | null>(null);

export const useTheme = (): ThemeContextValue => {
  const context = useContext(ThemeContext);
  if (!context)
    throw new Error('useTheme must be used within ThemeProvider');

  return context;
};
