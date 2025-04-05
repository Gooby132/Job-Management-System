import { notifications } from "@mantine/notifications";
import { ErrorDto } from "../commons/contracts";

type NotifyErrors = {
  errors: ErrorDto[];
};

export const notifyErrors = ({ errors }: NotifyErrors) => {
  if (errors) {
    errors.forEach((error) => {
      notifications.show({
        title: `Error Code - ${error.errorCode}`,
        message: error.message,
        color: "red",
      });
    });
  }
};
