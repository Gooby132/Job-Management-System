import {
  Box,
  Button,
  Container,
  Group,
  List,
  LoadingOverlay,
  Stack,
  Text,
  Textarea,
  Title,
} from "@mantine/core";
import { useNavigate, useParams } from "react-router-dom";
import { useGetJob } from "../../hooks/useGetJob";
import { useEffect, useState } from "react";
import dayjs from "dayjs";
import { useStartJob } from "../../hooks/useStartJob";
import { useRequestStopJob } from "../../hooks/useRequestStopJob";
import { useRestartJob } from "../../hooks/useRestartJob";
import { useDeleteJob } from "../../hooks/useDeleteJob";
import { AlertJobOperationModal } from "../organiems/modals/AlertJobOperationModal";
import { JobStatusMark } from "../atoms/JobStatusMark";
import { JobPriorityMark } from "../atoms/JobPriorityMark";
import { PageTitle } from "../atoms/PageTitle";

type Props = {};

export const Job = ({}: Props) => {
  const navigate = useNavigate();
  const [alertModal, setAlertModal] = useState<{
    operation: "start" | "stop" | "restart" | "delete";
  }>();
  const params = useParams<{ jobName: string }>();
  const [getJob, getJobIsLoading, getJobResponse] = useGetJob();
  const [startJob, startJobIsLoading] = useStartJob();
  const [requestStopJob, requestStopJobIsLoading] = useRequestStopJob();
  const [restartJob, restartJobIsLoading] = useRestartJob();
  const [deleteJob, deleteJobIsLoading] = useDeleteJob();

  if (!params.jobName) {
    navigate("/404", { replace: true });
    return <></>;
  }

  useEffect(() => {
    getJob({ jobName: params.jobName! });
  }, [params.jobName]);

  const handleOperationClicked = (
    operation: "start" | "stop" | "restart" | "delete"
  ) => {
    setAlertModal({
      operation,
    });
  };

  const loadingVisible =
    getJobIsLoading ||
    startJobIsLoading ||
    requestStopJobIsLoading ||
    restartJobIsLoading ||
    deleteJobIsLoading;

  return (
    <Container>
      <LoadingOverlay visible={loadingVisible} />
      <Stack gap={"lg"}>
        <PageTitle title={`Job - ${getJobResponse?.job?.name}`} />

        <Box>
          <Group justify="space-between">
            <Group>
              <Button onClick={() => handleOperationClicked("start")}>
                Start
              </Button>
              <Button onClick={() => handleOperationClicked("stop")}>
                Stop
              </Button>
              <Button onClick={() => handleOperationClicked("restart")}>
                Restart
              </Button>
            </Group>

            <Box>
              <Button
                color="red"
                onClick={() => handleOperationClicked("delete")}
              >
                Delete
              </Button>
            </Box>
          </Group>
        </Box>

        <Box>
          <Title order={3}>Job Details</Title>
          <List>
            <List.Item>
              <Text>Job Name: {getJobResponse?.job?.name}</Text>
            </List.Item>
            <List.Item>
              <Text>
                Job Priority:{" "}
                {
                  <JobPriorityMark
                    jobPriority={getJobResponse?.job?.priorityValue ?? 0}
                  />
                }
              </Text>
            </List.Item>
            <List.Item>
              <Text>
                Job Status:{" "}
                <JobStatusMark
                  jobStatus={getJobResponse?.job?.statusValue ?? 0}
                />
              </Text>
            </List.Item>
            <List.Item>
              <Text>
                Job Created In:{" "}
                {dayjs(getJobResponse?.job?.createdInUtc).format(
                  `DD/MM/YYYY HH:mm:ss`
                )}
              </Text>
            </List.Item>
            <List.Item>
              <Text>
                Job Execution Time:{" "}
                {dayjs(getJobResponse?.job?.executionTimeInUtc).format(
                  `DD/MM/YYYY HH:mm:ss`
                )}
              </Text>
            </List.Item>
            <List.Item>
              <Text>
                Job Start Time:{" "}
                {getJobResponse?.job?.startTimeInUtc != null
                  ? dayjs(getJobResponse?.job?.startTimeInUtc).format(
                      `DD/MM/YYYY HH:mm:ss`
                    )
                  : "-"}
              </Text>
            </List.Item>
            <List.Item>
              <Text>
                Job End Time:{" "}
                {getJobResponse?.job?.endTimeInUtc != null
                  ? dayjs(getJobResponse?.job?.endTimeInUtc).format(
                      `DD/MM/YYYY HH:mm:ss`
                    )
                  : "-"}
              </Text>
            </List.Item>
          </List>
        </Box>

        <Box>
          <Text>Job Log</Text>
          <Textarea rows={10} readOnly value={getJobResponse?.job?.log} />
        </Box>
      </Stack>

      <AlertJobOperationModal
        opened={alertModal !== undefined}
        operation={alertModal?.operation ?? "error"}
        onCancel={() => setAlertModal(undefined)}
        jobName={params.jobName!}
        onConfirm={() => {
          if (alertModal?.operation === "start") {
            startJob({ jobName: params.jobName! });
          } else if (alertModal?.operation === "stop") {
            requestStopJob({ jobName: params.jobName! });
          } else if (alertModal?.operation === "restart") {
            restartJob({ jobName: params.jobName! });
          } else if (alertModal?.operation === "delete") {
            deleteJob({ jobName: params.jobName! });
          }
          setAlertModal(undefined);
        }}
      ></AlertJobOperationModal>
    </Container>
  );
};
