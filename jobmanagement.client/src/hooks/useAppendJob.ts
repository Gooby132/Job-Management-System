import { useState } from "react";
import {
  AppendJobRequest,
  AppendJobResponse,
} from "../services/jobManager/contracts/jobManagerContracts";
import { jobManagerRestClient } from "../services/jobManager/rest/jobManagerRestClient";
import { notifyErrors } from "../services/helpers/notifier";
import { useDispatch } from "react-redux";
import { jobsActions } from "../redux/features/jobs/jobManagerSlice";

export const useAppendJob = (): [
  (request: AppendJobRequest) => Promise<void>,
  boolean,
  AppendJobResponse | undefined
] => {
  const dispatch = useDispatch();
  const [isLoading, setIsLoading] = useState(false);
  const [response, setResponse] = useState<AppendJobResponse>();

  const appendJob = async (request: AppendJobRequest) => {
    setIsLoading(true);
    setResponse(undefined);

    const result = await jobManagerRestClient.appendJob(request);

    if(result.errors)
      notifyErrors({ errors: result.errors });

    if(result.job) {
      dispatch(jobsActions.appendJob(result));
    }

    setIsLoading(false);
    setResponse(result);
  };

  return [appendJob, isLoading, response];
};
