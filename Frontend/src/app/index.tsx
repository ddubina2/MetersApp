import '@shared/index.css';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Router } from './providers/RouterProvider';
import { client } from '@shared/graphql/client';
import { ApolloProvider } from '@apollo/client/react';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ApolloProvider client={client}>
      <Router />
    </ApolloProvider>
  </StrictMode>,
);
