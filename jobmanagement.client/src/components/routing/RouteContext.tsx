import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { LANDING_ROUTE, DASHBOARD_ROUTE, LOGIN_ROUTE } from "./Routes";
import { App } from "../app/App";
import { Landing } from "../pages/Landing.tsx";
import { Jobs } from "../pages/Jobs.tsx";
import { Login } from "../pages/Login.tsx";
import { PropsWithChildren } from "react";

type Props = {};

export const RoutingContext = (props: PropsWithChildren) => {
  const router = createBrowserRouter([
    {
      element: <App />,
      children: [
        {
          path: LANDING_ROUTE,
          element: <Landing />,
        },
        {
          path: DASHBOARD_ROUTE,
          element: <Jobs />,
        },
        {
          path: LOGIN_ROUTE,
          element: <Login />,
        },
      ],
    },
  ]);

  return <RouterProvider router={router} />;
};