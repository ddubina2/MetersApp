import { useCallback, useEffect, useState, type FC, type PropsWithChildren } from 'react';
import { ThemeContext, type Theme, type ThemePreference } from './theme-context';

const STORAGE_KEY = 'meters-app-theme';

const getSystemTheme = (): Theme =>
  window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';

const getInitialTheme = (): Theme => {
  const saved = localStorage.getItem(STORAGE_KEY);
  if (saved === 'light' || saved === 'dark')
    return saved;

  return getSystemTheme();
};

export const ThemeProvider: FC<PropsWithChildren> = ({ children }) => {
  const [theme, setThemeState] = useState<Theme>(getInitialTheme);

  useEffect(() => {
    document.documentElement.classList.toggle('dark', theme === 'dark');
  }, [theme]);

  useEffect(() => {
    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
    const handleChange = () => {
      if (!localStorage.getItem(STORAGE_KEY))
        setThemeState(mediaQuery.matches ? 'dark' : 'light');
    };

    mediaQuery.addEventListener('change', handleChange);

    return () => mediaQuery.removeEventListener('change', handleChange);
  }, []);

  const setTheme = useCallback((preference: ThemePreference) => {
    if (preference === 'system') {
      localStorage.removeItem(STORAGE_KEY);
      setThemeState(getSystemTheme());
      return;
    }

    localStorage.setItem(STORAGE_KEY, preference);
    setThemeState(preference);
  }, []);

  const toggleTheme = useCallback(() => {
    setTheme(theme === 'dark' ? 'light' : 'dark');
  }, [setTheme, theme]);

  return (
    <ThemeContext.Provider value={{ theme, setTheme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
};
