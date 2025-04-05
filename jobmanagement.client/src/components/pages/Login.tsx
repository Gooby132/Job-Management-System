import { Container, LoadingOverlay, Title } from "@mantine/core"
import { LoginForm } from "../organiems/forms/LoginForm"
import { useLogin } from "../../hooks/useLogin"

type Props = {}

export const Login = ({}: Props) => {
  const [login, isLoading, response] = useLogin()

  return (
    <Container>
      <Title>Login</Title>
      <LoadingOverlay visible={isLoading} />
      <LoginForm onSubmit={login} />
    </Container>
  )
}