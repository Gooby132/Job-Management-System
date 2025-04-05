import { useState } from "react";
import {
  RequestStopJobRequest,
  RequestStopJobResponse,
} from "../services/jobManager/contracts/jobManagerContracts";
import { jobManagerRestClient } from "../services/jobManager/rest/jobManagerRestClient";
import { notifyErrors } from "../services/helpers/notifier";

export const useRequestStopJob = (): [
  (request: RequestStopJobRequest) => Promise<void>,
  boolean,
  RequestStopJobResponse | undefined
] => {
  const [isLoading, setIsLoading] = useState(false);
  const [response, setResponse] = useState<RequestStopJobResponse>();

  const requestStopJob = async (request: RequestStopJobRequest) => {
    setIsLoading(true);
    setResponse(undefined);

    const result = await jobManagerRestClient.requestStopJob(request);

    if (result.errors) {
      notifyErrors({ errors: result.errors });
    }

    setIsLoading(false);
    setResponse(result);
  };

  return [requestStopJob, isLoading, response];
};
