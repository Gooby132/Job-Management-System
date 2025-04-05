import { Center, Container } from "@mantine/core";

type Props = {};

export const NotFound = ({}: Props) => {
  return (
    <Container>
      <Center>
        <h1>404 - Page Not Found</h1>
        <p>The page you are looking for does not exist.</p>
        <p>Please check the URL or return to the homepage.</p>
      </Center>
    </Container>
  );
};
