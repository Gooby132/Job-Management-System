import { useState } from "react";
import {
  GetAvailableJobsExecutionsResponse,
  GetAvailableJobsExecutionsRequest,
} from "../services/jobManager/contracts/jobManagerContracts";
import { jobManagerRestClient } from "../services/jobManager/rest/jobManagerRestClient";

export const useAvailableJobsExecutions = (): [
  (request: GetAvailableJobsExecutionsRequest) => Promise<void>,
  boolean,
  GetAvailableJobsExecutionsResponse | undefined
] => {
  const [isLoading, setIsLoading] = useState(false);
  const [response, setResponse] =
    useState<GetAvailableJobsExecutionsResponse>();

  const availableJobsExecutions = async (
    request: GetAvailableJobsExecutionsRequest
  ) => {
    setIsLoading(true);
    setResponse(undefined);

    const result = await jobManagerRestClient.getAvailableJobsExecutions(
      request
    );

    setIsLoading(false);
    setResponse(result);
  };

  return [availableJobsExecutions, isLoading, response];
};
