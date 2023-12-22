import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Counter } from "./pages/Counter";
import { FetchData } from "./pages/FetchData";
import { Home } from "./pages/Home";
import { CrudRentSubmissionPage } from "./pages/CrudRentSubmissionPage";

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
    path: '/new-rent-submission',
    requireAuth: true,
    element: <CrudRentSubmissionPage />
  },
  {
    path: '/rent-submission/:id',
    requireAuth: true,
    element: <CrudRentSubmissionPage />
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
