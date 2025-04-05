import { useState } from "react";
import {
  JobsStatusesRequest,
  JobsStatusesResponse,
} from "../services/jobManager/contracts/jobManagerContracts";
import { jobManagerRestClient } from "../services/jobManager/rest/jobManagerRestClient";
import { useDispatch } from "react-redux";
import { notifyErrors } from "../services/helpers/notifier";
import { jobsActions } from "../redux/features/jobs/jobManagerSlice";

export const useJobStatuses = (): [
  (request: JobsStatusesRequest) => Promise<void>,
  boolean,
  JobsStatusesResponse | undefined
] => {
  const dispatch = useDispatch();
  const [isLoading, setIsLoading] = useState(false);
  const [response, setResponse] = useState<JobsStatusesResponse>();

  const getJobsStatuses = async (request: JobsStatusesRequest) => {
    setIsLoading(true);
    setResponse(undefined);

    const result = await jobManagerRestClient.jobsStatuses(request);

    if(result.errors)
      notifyErrors({ errors: result.errors });

    if(!result.errors)
      dispatch(jobsActions.setJobStatuses(result));

    setIsLoading(false);
    setResponse(result);
  };

  return [getJobsStatuses, isLoading, response];
};
