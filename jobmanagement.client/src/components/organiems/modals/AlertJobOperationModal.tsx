import { Button, Group, Modal, Stack, Text } from "@mantine/core";

type Props = {
  operation: string;
  jobName: string;
  onCancel: () => void;
  onConfirm: () => void;
  opened: boolean;
};

export const AlertJobOperationModal = ({
  operation,
  jobName,
  onCancel,
  onConfirm,
  opened,
}: Props) => {
  const getOperationText = () => {
    switch (operation) {
      case "start":
        return `Are you sure you want to start job ${jobName}?`;
      case "stop":
        return `Are you sure you want to stop job ${jobName}?`;
      case "restart":
        return `Are you sure you want to restart job ${jobName}?`;
      case "delete":
        return `Are you sure you want to delete job ${jobName}?`;
      default:
        return `Are you sure you want to perform this operation on job ${jobName}?`;
    }
  };

  return (
    <Modal
      opened={opened}
      onClose={onCancel}
      title={`Confirm Operation - "${operation}"`}
      withCloseButton={false}
    >
      <Stack>
        <Text>{getOperationText()}</Text>
        <Group>
          <Button color="red" onClick={onConfirm}>
            Confirm
          </Button>
          <Button onClick={onCancel}>Cancel</Button>
        </Group>
      </Stack>
    </Modal>
  );
};
