import { Box, Text } from "@mantine/core";
import { ErrorDto } from "../../services/commons/contracts";

type Props = {
  errors?: ErrorDto[];
};

export const ErrorsDisplay = ({ errors }: Props) => {
  return errors?.map((error, val) => (
    <Box bg={"red.2"} key={`${error.errorCode} - ${val}`} p={"xs"}>
      <Text c={"red.9"}>
        {error.groupCode} - {error.errorCode}:
      </Text>
      <Text c={"red.7"}>{error.message}</Text>
    </Box>
  )) ?? <></>;
};
