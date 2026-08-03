import '@shared/index.css';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Router } from './providers/RouterProvider';
import { ThemeProvider } from './providers/ThemeProvider';
import { client } from '@shared/graphql/client';
import { ApolloProvider } from '@apollo/client/react';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ThemeProvider>
      <ApolloProvider client={client}>
        <Router />
      </ApolloProvider>
    </ThemeProvider>
  </StrictMode>,
);
