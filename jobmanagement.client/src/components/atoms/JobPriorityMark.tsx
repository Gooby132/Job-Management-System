import { Mark } from "@mantine/core";
import { JobPriority } from "../../services/jobManager/contracts/jobManagerContracts";

type Props = {
  jobPriority: JobPriority;
};

export const JobPriorityMark = ({ jobPriority }: Props) => {
  const statusColors = (jobPriority: JobPriority) => {
    switch (jobPriority) {
      case JobPriority.High:
        return "blue.3";
      case JobPriority.Regular:
        return "blue.1";
      default:
        return undefined;
    }
  };

  return (
    <Mark color={statusColors(jobPriority)}>{JobPriority[jobPriority]}</Mark>
  );
};
