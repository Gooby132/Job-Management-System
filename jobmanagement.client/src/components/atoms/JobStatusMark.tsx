import { Mark } from "@mantine/core";
import { JobStatus } from "../../services/jobManager/contracts/jobManagerContracts";

type Props = {
  jobStatus: JobStatus;
};

export const JobStatusMark = ({ jobStatus }: Props) => {
  const statusColors = (jobStatus: JobStatus) => {
    switch (jobStatus) {
      case JobStatus.Running:
        return "blue";
      case JobStatus.Completed:
        return "green";
      case JobStatus.Failed:
        return "red";
      case JobStatus.Stopping:
        return "orange";
      case JobStatus.Canceled:
        return "gray";
      default:
        return undefined;
    }
  };

  return <Mark color={statusColors(jobStatus)}>{JobStatus[jobStatus]}</Mark>;
};
