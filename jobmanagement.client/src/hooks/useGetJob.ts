import { useState } from "react";
import { GetJobRequest, GetJobResponse } from "../services/jobManager/contracts/jobManagerContracts";
import { jobManagerRestClient } from "../services/jobManager/rest/jobManagerRestClient";
import { notifyErrors } from "../services/helpers/notifier";

export const useGetJob = () : [
  (request: GetJobRequest) => Promise<void>,
  boolean,
  GetJobResponse | undefined
] => {
  const [isLoading, setIsLoading] = useState(false);
  const [response, setResponse] =
    useState<GetJobResponse>();

  const getJob = async (request: GetJobRequest) => {
    setIsLoading(true);
    setResponse(undefined);

    const result = await jobManagerRestClient.getJob(
      request
    );

    if(result.errors) {
      notifyErrors({ errors: result.errors });
    }

    setIsLoading(false);
    setResponse(result);
  };

  return [getJob, isLoading, response];
}