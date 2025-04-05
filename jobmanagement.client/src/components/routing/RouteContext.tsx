import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { LANDING_ROUTE, DASHBOARD_ROUTE, LOGIN_ROUTE, JOB_ROUTE } from "./Routes";
import { App } from "../app/App";
import { Landing } from "../pages/Landing.tsx";
import { Dashboard } from "../pages/Dashboard.tsx";
import { Login } from "../pages/Login.tsx";
import { Job } from "../pages/Job.tsx";

type Props = {};

export const RoutingContext = ({}: Props) => {
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
          element: <Dashboard />,
        },
        {
          path: LOGIN_ROUTE,
          element: <Login />,
        },
        {
          path: `${JOB_ROUTE}/:jobName`,
          element: <Job />,
        },
      ],
    },
  ]);

  return <RouterProvider router={router} />;
};