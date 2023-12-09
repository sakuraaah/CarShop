import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Counter } from "./components/pages/Counter";
import { FetchData } from "./components/pages/FetchData";
import { Home } from "./components/pages/Home";
import { NewProductPage } from "./components/pages/NewProductPage";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    requireAuth: true,
    element: <FetchData />
  },
  {
    path: '/new-product',
    element: <NewProductPage />
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
