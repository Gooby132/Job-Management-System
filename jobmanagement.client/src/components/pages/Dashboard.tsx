import {
  Box,
  Button,
  Container,
  Group,
  LoadingOverlay,
  Stack,
  Table,
} from "@mantine/core";
import { useJobStatuses } from "../../hooks/useJobStatuses";
import { useEffect } from "react";
import { useDisclosure } from "@mantine/hooks";
import { AppendJobModal } from "../organiems/modals/AppendJobModal";
import { AppendJobRequest } from "../../services/jobManager/contracts/jobManagerContracts";
import { useAppendJob } from "../../hooks/useAppendJob";
import { Link } from "react-router-dom";
import { JOB_ROUTE } from "../routing/Routes";
import { JobStatusMark } from "../atoms/JobStatusMark";
import { JobPriorityMark } from "../atoms/JobPriorityMark";
import { useSelector } from "react-redux";
import { RootState } from "../../redux/store";
import { DateTimeDisplay } from "../atoms/DateTimeDisplay";
import { PageTitle } from "../atoms/PageTitle";

type Props = {};

export const Dashboard = ({}: Props) => {
  const [appendJob, appendJobIsLoading] = useAppendJob();
  const [getJobsStatuses, jobStatusesIsLoading] = useJobStatuses();
  const jobs = useSelector((state: RootState) => state.jobsSlice).jobs;

  const [
    appendJobModalIsOpen,
    { close: appendJobModalClose, open: appendJobModalOpen },
  ] = useDisclosure();

  useEffect(() => {
    getJobsStatuses({});
  }, []);

  const appendModalOnSubmit = (request: AppendJobRequest) => {
    appendJobModalClose();
    appendJob(request);
  };

  const rows = jobs?.map((job) => (
    <Table.Tr key={job.name} ta={"center"}>
      <Table.Td>{job.name}</Table.Td>
      <Table.Td>{job.executionName}</Table.Td>
      <Table.Td>{<JobPriorityMark jobPriority={job.priorityValue} />}</Table.Td>
      <Table.Td>
        <JobStatusMark jobStatus={job.statusValue} />
      </Table.Td>
      <Table.Td>{job.progress}%</Table.Td>
      <Table.Td>
        <DateTimeDisplay dateTime={job.createdInUtc} />
      </Table.Td>
      <Table.Td>
        <DateTimeDisplay dateTime={job.executionTimeInUtc} />
      </Table.Td>
      <Table.Td>
        <DateTimeDisplay dateTime={job.startTimeInUtc} />
      </Table.Td>
      <Table.Td>
        <DateTimeDisplay dateTime={job.endTimeInUtc} />
      </Table.Td>
      <Table.Td>
        <Button component={Link} to={`${JOB_ROUTE}/${job.name}`}>
          Details
        </Button>
      </Table.Td>
    </Table.Tr>
  ));

  return (
    <Container>
      <Stack pt={"lg"} gap={"lg"}>
        <PageTitle title={"Job Dashboard"} />

        <Group>
          <Button onClick={appendJobModalOpen}>Add Job</Button>
        </Group>

        <Box>
          <LoadingOverlay
            visible={jobStatusesIsLoading || appendJobIsLoading}
          />
          <Table.ScrollContainer minWidth={500}>
            <Table
              striped
              withColumnBorders
              withTableBorder
              stickyHeaderOffset={60}
            >
              <Table.Thead>
                <Table.Tr>
                  <Table.Th>Job Name</Table.Th>
                  <Table.Th>Execution Name</Table.Th>
                  <Table.Th>Priority</Table.Th>
                  <Table.Th>Status</Table.Th>
                  <Table.Th>Progress</Table.Th>
                  <Table.Th>Created On</Table.Th>
                  <Table.Th>Execution Time</Table.Th>
                  <Table.Th>Start Time</Table.Th>
                  <Table.Th>End Time</Table.Th>
                  <Table.Th></Table.Th>
                </Table.Tr>
              </Table.Thead>
              <Table.Tbody>{rows}</Table.Tbody>
            </Table>
          </Table.ScrollContainer>
        </Box>
      </Stack>

      <AppendJobModal
        onClose={appendJobModalClose}
        opened={appendJobModalIsOpen}
        onSubmit={appendModalOnSubmit}
      />
    </Container>
  );
};
