import { Modal } from "@mantine/core"
import { AppendJobRequest } from "../../../services/jobManager/contracts/jobManagerContracts"
import { AppendJobForm } from "../forms/AppendJobForm"

type Props = {
  opened: boolean,
  onClose: () => void,
  onSubmit: (request: AppendJobRequest) => void,
}

export const AppendJobModal = ({ onClose, onSubmit, opened }: Props) => {
  return (
    <Modal opened={opened} onClose={onClose} title="Append Job" withCloseButton={false} >
      <AppendJobForm onSubmit={onSubmit} />
    </Modal>
  )
}