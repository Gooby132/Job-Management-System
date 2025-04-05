import {
  Box,
  Button,
  Combobox,
  Input,
  InputBase,
  LoadingOverlay,
  Stack,
  TextInput,
  useCombobox,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { AppendJobRequest } from "../../../services/jobManager/contracts/jobManagerContracts";
import { useAvailableJobsExecutions } from "../../../hooks/useAvailableJobsExecutions";
import { useEffect } from "react";
import { ErrorsDisplay } from "../../molecules/ErrorsDisplay";
import { jobManagerRules } from "../../../rules/jobManagerRules";
import { DateTimePicker } from "@mantine/dates";

type Props = {
  onSubmit: (values: AppendJobRequest) => void;
};

const AVAILABLE_PRIORITIES = [
  { value: 0, label: "Choose priority" },
  { value: 1, label: "High" },
  { value: 2, label: "Regular" },
];

export const AppendJobForm = ({ onSubmit }: Props) => {
  const form = useForm<AppendJobRequest>({
    initialValues: {
      jobName: "",
      jobExecutionName: "",
      priorityValue: 0,
      executionTimeInUtc: "",
    },
    validate: {
      jobName: jobManagerRules.job.jobName,
      jobExecutionName: jobManagerRules.job.executionName,
      priorityValue: jobManagerRules.job.jobPriority,
      executionTimeInUtc: jobManagerRules.job.executionTime,
    },
  });

  const [
    availableJobsExecutions,
    availableJobsExecutionsIsLoading,
    availableJobsExecutionsResponse,
  ] = useAvailableJobsExecutions();

  useEffect(() => {
    availableJobsExecutions({});
  }, []);

  const availableJobsComboboxStore = useCombobox();
  const priorityComboboxStore = useCombobox();

  const jobExecutionsOptions =
    availableJobsExecutionsResponse?.jobExecutions?.map((job) => (
      <Combobox.Option key={job} value={String(job)}>
        {job}
      </Combobox.Option>
    ));

  const priorityOptions = AVAILABLE_PRIORITIES.map((priority) => (
    <Combobox.Option key={priority.value} value={String(priority.value)}>
      {priority.label}
    </Combobox.Option>
  ));

  const isSubmitDisabled =
    (availableJobsExecutionsResponse?.errors?.length ?? 0) > 0;

  return (
    <Box
      component="form"
      onSubmit={form.onSubmit(onSubmit, (err) => console.log(err))}
    >
      <LoadingOverlay visible={availableJobsExecutionsIsLoading} />
      <Stack>
        <TextInput
          key={form.key("jobName")}
          label="Job Name"
          {...form.getInputProps("jobName")}
        />

        <Combobox
          store={availableJobsComboboxStore}
          onOptionSubmit={(val) => {
            form.setFieldValue("jobExecutionName", val);
            availableJobsComboboxStore.closeDropdown();
          }}
        >
          <Combobox.Target>
            <InputBase
              component="button"
              type="button"
              label="Job Executions"
              pointer
              rightSection={<Combobox.Chevron />}
              onClick={() => availableJobsComboboxStore.toggleDropdown()}
              rightSectionPointerEvents="none"
              error={form.errors.jobExecutionName}
            >
              {form.values.jobExecutionName || (
                <Input.Placeholder>Pick value</Input.Placeholder>
              )}
            </InputBase>
          </Combobox.Target>

          <Combobox.Dropdown>
            <Combobox.Options>{jobExecutionsOptions}</Combobox.Options>
          </Combobox.Dropdown>
        </Combobox>

        <Combobox
          store={priorityComboboxStore}
          onOptionSubmit={(val) => {
            form.setFieldValue("priorityValue", Number.parseInt(val));
            priorityComboboxStore.closeDropdown();
          }}
        >
          <Combobox.Target>
            <InputBase
              component="button"
              type="button"
              label="Priority"
              pointer
              rightSection={<Combobox.Chevron />}
              onClick={() => priorityComboboxStore.toggleDropdown()}
              rightSectionPointerEvents="none"
              error={form.errors.priorityValue}
            >
              {AVAILABLE_PRIORITIES[form.values.priorityValue].label || (
                <Input.Placeholder>Pick value</Input.Placeholder>
              )}
            </InputBase>
          </Combobox.Target>

          <Combobox.Dropdown>
            <Combobox.Options>{priorityOptions}</Combobox.Options>
          </Combobox.Dropdown>
        </Combobox>

        <DateTimePicker
          key={form.key("executionTimeInUtc")}
          label="Execution Time"
          minDate={new Date()}
          {...form.getInputProps("executionTimeInUtc")}
        />

        <Button type="submit" disabled={isSubmitDisabled}>
          Submit
        </Button>

        <ErrorsDisplay errors={availableJobsExecutionsResponse?.errors} />
      </Stack>
    </Box>
  );
};
