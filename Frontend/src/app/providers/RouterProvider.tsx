import { ROUTES } from '@app/router/routes';
import { Layout } from '@widgets/Layout';
import { Route, Switch, Router as WouterRouter } from 'wouter';

export const Router = () => {

  return (
    <WouterRouter>
      <Switch>
        {ROUTES.map(route => {
            return (
              <Route key={route.path} path={route.path}>
                <Layout>
                  <route.content />
                </Layout>
              </Route>
            );
          })}
      </Switch>
    </WouterRouter>
  );
};
