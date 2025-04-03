import { AppShell, Button, Group, MantineProvider, Text } from "@mantine/core";
import { Notifications } from "@mantine/notifications";
import { Link, Outlet } from "react-router-dom";
import { DASHBOARD_ROUTE, LANDING_ROUTE, LOGIN_ROUTE } from "../routing/Routes";

export const App = () => {
  const HEADER_SIZE = 64;

  return (
    <MantineProvider>
      <Notifications />

      <AppShell
        header={{
          height: HEADER_SIZE,
        }}
      >
        <AppShell.Header>
          <Group p={10} justify="space-between" h={HEADER_SIZE}>
            <Button component={Link} to={LANDING_ROUTE}>
              <Text>Home</Text>
            </Button>

            <Group>
              <Button component={Link} to={DASHBOARD_ROUTE}>
                <Text>Dashboard</Text>
              </Button>

              <Button component={Link} to={LOGIN_ROUTE}>
                <Text>Login</Text>
              </Button>
            </Group>
          </Group>
        </AppShell.Header>
        <AppShell.Main>
          <Outlet />
        </AppShell.Main>
      </AppShell>
    </MantineProvider>
  );
};
