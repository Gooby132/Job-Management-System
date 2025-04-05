import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { MantineProvider } from "@mantine/core";
import { RoutingContext } from "./components/routing/RouteContext";
import { Provider } from "react-redux";
import store from "./redux/store";
import "@mantine/core/styles.css";
import "@mantine/notifications/styles.css";
import "@mantine/dates/styles.css";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Provider store={store}>
      <MantineProvider>
        <RoutingContext />
      </MantineProvider>
    </Provider>
  </StrictMode>
);
