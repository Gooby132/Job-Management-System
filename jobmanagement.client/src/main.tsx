import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { MantineProvider } from "@mantine/core";
import { RoutingContext } from "./components/routing/RouteContext";
import "@mantine/core/styles.css";
import '@mantine/notifications/styles.css';

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <MantineProvider>
      <RoutingContext />
    </MantineProvider>
  </StrictMode>
);
