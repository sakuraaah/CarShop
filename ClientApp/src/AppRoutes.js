import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';

import { ProfilePage } from "./pages/ProfilePage";

import { CrudRentSubmissionPage } from "./pages/CrudRentSubmissionPage";
import { RentSubmissionList } from "./pages/RentSubmissionList";

import { CrudRentItemPage } from "./pages/CrudRentItemPage";
import { RentItemList } from "./pages/RentItemList";
import { RentItemSellerList } from "./pages/RentItemSellerList";

import { CrudBuyItemPage } from "./pages/CrudBuyItemPage";
import { BuyItemList } from "./pages/BuyItemList";
import { BuyItemSellerList } from "./pages/BuyItemSellerList";

const AppRoutes = [
  {
    path: '/profile',
    requireAuth: true,
    element: <ProfilePage />
  },
  {
    path: '/profile/edit',
    requireAuth: true,
    element: <ProfilePage />
  },
  {
    path: '/new-buy-item',
    requireAuth: true,
    element: <CrudBuyItemPage />
  },
  {
    path: '/buy-item/:id',
    element: <CrudBuyItemPage />
  },
  {
    index: true,
    element: <BuyItemList />
  },
  {
    path: '/profile/for-sale',
    requireAuth: true,
    element: <BuyItemSellerList />
  },
  {
    path: '/new-rent-item',
    requireAuth: true,
    element: <CrudRentItemPage />
  },
  {
    path: '/rent-item/:id',
    element: <CrudRentItemPage />
  },
  {
    path: '/rental',
    element: <RentItemList />
  },
  {
    path: '/profile/rental',
    requireAuth: true,
    element: <RentItemSellerList />
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
  {
    path: '/rent-submissions',
    requireAuth: true,
    element: <RentSubmissionList />
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
