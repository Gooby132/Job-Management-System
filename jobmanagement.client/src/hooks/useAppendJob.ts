import { useState } from "react";
import {
  AppendJobRequest,
  AppendJobResponse,
} from "../services/jobManager/contracts/jobManagerContracts";
import { jobManagerRestClient } from "../services/jobManager/rest/jobManagerRestClient";
import { notifyErrors } from "../services/helpers/notifier";

export const useAppendJob = (): [
  (request: AppendJobRequest) => Promise<void>,
  boolean,
  AppendJobResponse | undefined
] => {
  const [isLoading, setIsLoading] = useState(false);
  const [response, setResponse] = useState<AppendJobResponse>();

  const appendJob = async (request: AppendJobRequest) => {
    setIsLoading(true);
    setResponse(undefined);

    const result = await jobManagerRestClient.appendJob(request);

    if(result.errors)
      notifyErrors({ errors: result.errors });

    setIsLoading(false);
    setResponse(result);
  };

  return [appendJob, isLoading, response];
};
