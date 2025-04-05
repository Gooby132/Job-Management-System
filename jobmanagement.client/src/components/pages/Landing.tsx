import { Box, Container, List, Stack, Text, Title } from "@mantine/core";

type Props = {};

export const Landing = ({}: Props) => {
  return (
    <Container>
      <Title>Job Management Service</Title>
      <Stack>
        <Box>
          <Title>Overview</Title>
          <Text>
            Our job management service is designed to handle the execution of
            jobs, monitor their state, and ensure efficient processing. It
            allows users to submit executables and track their progress in
            real-time.
          </Text>
        </Box>
        <Box>
          <Title>Key Features</Title>
          <List>
            <List.Item>Submit and execute jobs dynamically</List.Item>
            <List.Item>
              Monitor job states (Pending, Running, Completed, Failed)
            </List.Item>
            <List.Item>Handle concurrency and resource allocation</List.Item>
            <List.Item>
              Log execution details for auditing and debugging
            </List.Item>
          </List>
        </Box>
        <Box>
          <Title>How It Works</Title>
          <Text>
            Users submit executables through an API or web interface. The
            service schedules the job based on available resources and executes
            it. Throughout the process, users can monitor execution state and
            receive notifications upon completion or failure.
          </Text>
        </Box>
      </Stack>
    </Container>
  );
};
