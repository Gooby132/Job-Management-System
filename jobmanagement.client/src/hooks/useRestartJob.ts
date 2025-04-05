import { useState } from "react";
import {
  RestartJobRequest,
  RestartJobResponse,
} from "../services/jobManager/contracts/jobManagerContracts";
import { jobManagerRestClient } from "../services/jobManager/rest/jobManagerRestClient";
import { notifyErrors } from "../services/helpers/notifier";

export const useRestartJob = (): [
  (request: RestartJobRequest) => Promise<void>,
  boolean,
  RestartJobResponse | undefined
] => {
  const [isLoading, setIsLoading] = useState(false);
  const [response, setResponse] = useState<RestartJobResponse>();

  const restartJob = async (request: RestartJobRequest) => {
    setIsLoading(true);
    setResponse(undefined);

    const result = await jobManagerRestClient.deleteJob(request);

    if (result.errors) {
      notifyErrors({ errors: result.errors });
    }

    setIsLoading(false);
    setResponse(result);
  };

  return [restartJob, isLoading, response];
};
