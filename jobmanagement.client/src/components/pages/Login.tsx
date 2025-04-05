import { Container, LoadingOverlay, Stack, Title } from "@mantine/core";
import { LoginForm } from "../organiems/forms/LoginForm";
import { useLogin } from "../../hooks/useLogin";
import { PageTitle } from "../atoms/PageTitle";
import { useNavigate } from "react-router-dom";
import { DASHBOARD_ROUTE } from "../routing/Routes";

type Props = {};

export const Login = ({}: Props) => {
  const [login, isLoading, response] = useLogin();
  const navigate = useNavigate();

  if(response?.token)
    navigate(DASHBOARD_ROUTE);

  return (
    <Container>
      <Stack gap={"lg"}>
        <PageTitle title="Login" />
        <LoadingOverlay visible={isLoading} />
        <LoginForm onSubmit={login} />
      </Stack>
    </Container>
  );
};
