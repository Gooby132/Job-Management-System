import { useState } from "react";
import {
  StartJobResponse,
  StartJobRequest,
} from "../services/jobManager/contracts/jobManagerContracts";
import { jobManagerRestClient } from "../services/jobManager/rest/jobManagerRestClient";
import { notifyErrors } from "../services/helpers/notifier";

export const useStartJob = (): [
  (request: StartJobRequest) => Promise<void>,
  boolean,
  StartJobResponse | undefined
] => {
  const [isLoading, setIsLoading] = useState(false);
  const [response, setResponse] = useState<StartJobResponse>();

  const startJob = async (request: StartJobRequest) => {
    setIsLoading(true);
    setResponse(undefined);

    const result = await jobManagerRestClient.startJob(request);

    if (result.errors) {
      notifyErrors({ errors: result.errors });
    }

    setIsLoading(false);
    setResponse(result);
  };

  return [startJob, isLoading, response];
};
