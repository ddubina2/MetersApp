import { ApolloClient, InMemoryCache, HttpLink } from '@apollo/client';
import { envs } from '@shared/envs';

export const client = new ApolloClient({
  link: new HttpLink({
    uri: envs.VITE_API_BASE_URL,
  }),
  cache: new InMemoryCache(),
});
