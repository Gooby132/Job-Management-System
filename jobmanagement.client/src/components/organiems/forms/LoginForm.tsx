import { Box, Button, Stack, TextInput } from "@mantine/core";
import { useForm } from "@mantine/form";
import { LoginRequest } from "../../../services/jobManager/contracts/jobManagerContracts";

const USER_NAME_MAX_LENGTH = 20;
const USER_NAME_MIN_LENGTH = 2;

const PASSWORD_MAX_LENGTH = 20;
const PASSWORD_MIN_LENGTH = 20;

type Props = {
  onSubmit: (values: LoginRequest) => void;
};

export type LoginFormValues = {
  userName: string;
  password: string;
};

export const LoginForm = ({}: Props) => {
  const form = useForm<LoginRequest>({
    initialValues: {
      userName: "",
      password: "",
    },
    validate: {
      userName: (value) =>
        value.length < USER_NAME_MIN_LENGTH ||
        value.length > USER_NAME_MAX_LENGTH
          ? null
          : "Invalid username",
      password: (value) =>
        value.length < PASSWORD_MIN_LENGTH || value.length > PASSWORD_MAX_LENGTH
          ? null
          : "Invalid password",
    },
  });

  return (
    <Box
      component="form"
      onSubmit={form.onSubmit((values) => console.log(values))}
    >
      <Stack gap="lg">
        <TextInput
          key={form.key("userName")}
          label="Username"
          {...form.getInputProps("userName")}
        />

        <TextInput
          key={form.key("password")}
          type="password"
          label="Password"
          {...form.getInputProps("password")}
        />

        <Button type="submit">Submit</Button>
      </Stack>
    </Box>
  );
};
