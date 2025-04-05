import { useState } from "react";
import {
  DeleteJobResponse,
  DeleteJobRequest,
} from "../services/jobManager/contracts/jobManagerContracts";
import { jobManagerRestClient } from "../services/jobManager/rest/jobManagerRestClient";
import { notifyErrors } from "../services/helpers/notifier";

export const useDeleteJob = (): [
  (request: DeleteJobRequest) => Promise<void>,
  boolean,
  DeleteJobResponse | undefined
] => {
  const [isLoading, setIsLoading] = useState(false);
  const [response, setResponse] = useState<DeleteJobResponse>();

  const deleteJob = async (request: DeleteJobRequest) => {
    setIsLoading(true);
    setResponse(undefined);

    const result = await jobManagerRestClient.deleteJob(request);

    if (result.errors) {
      notifyErrors({ errors: result.errors });
    }

    setIsLoading(false);
    setResponse(result);
  };

  return [deleteJob, isLoading, response];
};
