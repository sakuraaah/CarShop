import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Counter } from "./pages/Counter";
import { FetchData } from "./pages/FetchData";
import { Home } from "./pages/Home";
import { NewProductPage } from "./pages/NewProductPage";

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
