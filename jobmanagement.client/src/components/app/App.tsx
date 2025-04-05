import {
  AppShell,
  Button,
  Group,
  Loader,
  MantineProvider,
  Text,
} from "@mantine/core";
import { Notifications } from "@mantine/notifications";
import { Link, Outlet } from "react-router-dom";
import { DASHBOARD_ROUTE, LANDING_ROUTE, LOGIN_ROUTE } from "../routing/Routes";
import { useConnectSignalR } from "../../hooks/useConnectSignalR";
import { useEffect } from "react";
import { useSelector } from "react-redux";
import { RootState } from "../../redux/store";

export const App = () => {
  const HEADER_SIZE = 64;
  const [connect, isConnecting, result] = useConnectSignalR();
  const user = useSelector((state: RootState) => state.userSlice);

  useEffect(() => {
    connect();
  }, []);

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
              {user.token === undefined ? (
                <Button component={Link} to={LOGIN_ROUTE}>
                  <Text>Login</Text>
                </Button>
              ) : (
                <Text>Hi {user.userName}</Text>
              )}
            </Group>
          </Group>
        </AppShell.Header>
        <AppShell.Main>
          <Outlet />
        </AppShell.Main>
        <AppShell.Footer>
          <Group>
            {isConnecting && (
              <Group align="center">
                <Loader size={10} />
                <Text c="yellow">Connecting</Text>
              </Group>
            )}
            {!isConnecting && result ? (
              <Text c="green">Connected</Text>
            ) : (
              <Text c="red">Failed</Text>
            )}
          </Group>
        </AppShell.Footer>
      </AppShell>
    </MantineProvider>
  );
};
