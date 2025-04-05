import { GenericResponse } from "../../commons/contracts";

export type JobManagerClient = {
  appendJob: (request: AppendJobRequest) => Promise<AppendJobResponse>;
  getAvailableJobsExecutions: (request: GetAvailableJobsExecutionsRequest) => Promise<GetAvailableJobsExecutionsResponse>;
  deleteJob: (request: DeleteJobRequest) => Promise<DeleteJobResponse>;
  jobsStatuses: (request: JobsStatusesRequest) => Promise<JobsStatusesResponse>;
  requestStopJob: (request: RequestStopJobRequest) => Promise<RequestStopJobResponse>;
  restartJob: (request: RestartJobRequest) => Promise<RestartJobResponse>;
  startJob: (request: StartJobRequest) => Promise<StartJobResponse>;
  getJob: (request: GetJobRequest) => Promise<GetJobResponse>;
}

// get job
export type GetJobRequest = {
  jobName: string;
}

export type GetJobResponse = {
  job?: JobDto;
} & GenericResponse;

// append all jobs

export type AppendJobRequest = {
  jobName: string;
  priorityValue: number;
  executionTimeInUtc: string;
  jobExecutionName: string;
}

export type AppendJobResponse = {
  job?: JobDto;
} & GenericResponse;

// available jobs 

export type GetAvailableJobsExecutionsRequest = {

}

export type GetAvailableJobsExecutionsResponse = {
  jobExecutions: string[];
} & GenericResponse;

// delete job 

export type DeleteJobRequest = {
  jobName: string;
}

export type DeleteJobResponse = {
  job?: JobDto;
} & GenericResponse;

// jobs statuses 

export type JobsStatusesRequest = {

}

export type JobsStatusesResponse = {
  jobs?: JobDto[]; 
} & GenericResponse;

// request stop job

export type RequestStopJobRequest = {
  jobName: string;
}

export type RequestStopJobResponse = {
  job?: JobDto;
} & GenericResponse;

// restart job 

export type RestartJobRequest = {
  jobName: string;
}

export type RestartJobResponse = {
  job?: JobDto;
} & GenericResponse;

// start job

export type StartJobRequest = {
  jobName: string;
}

export type StartJobResponse = {
  job?: JobDto;
} & GenericResponse;

// dtos

export type JobDto = {
  name: string;
  priorityValue: number;
  statusValue: number;
  progress: number;
  createdInUtc: string;
  executionTimeInUtc: string;
  startTimeInUtc?: string;
  endTimeInUtc?: string;
  log: string;
}

export enum JobPriority {
  Undefined = 0,
  High = 1,
  Regular = 2,
}

export enum JobStatus {
  Undefined = 0,
  Pending = 1,
  Running = 2,
  Completed = 3,
  Failed = 4,
  Canceled = 5,
  Stopping = 6,
  Restarting = 7,
  Stopped = 8,
  PendingDeletion = 9
}