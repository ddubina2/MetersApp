import '@shared/index.css';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Router } from './providers/RouterProvider';
import { ThemeProvider } from './providers/ThemeProvider';
import { I18nSync } from './providers/I18nSync';
import { client } from '@shared/graphql/client';
import { ApolloProvider } from '@apollo/client/react';
import '@i18n';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ThemeProvider>
      <ApolloProvider client={client}>
        <I18nSync />
        <Router />
      </ApolloProvider>
    </ThemeProvider>
  </StrictMode>,
);
