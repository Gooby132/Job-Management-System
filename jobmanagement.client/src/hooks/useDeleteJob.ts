import { useState } from "react";
import {
  DeleteJobResponse,
  DeleteJobRequest,
} from "../services/jobManager/contracts/jobManagerContracts";
import { jobManagerRestClient } from "../services/jobManager/rest/jobManagerRestClient";
import { notifyErrors } from "../services/helpers/notifier";
import { useSelector } from "react-redux";
import { RootState } from "../redux/store";
import { NOT_LOGGED_IN_ERROR_RESPONSE } from "../services/commons/contracts";

export const useDeleteJob = (): [
  (request: Omit<DeleteJobRequest, "userToken">) => Promise<void>,
  boolean,
  DeleteJobResponse | undefined
] => {
  const user = useSelector((state: RootState) => state.userSlice);
  const [isLoading, setIsLoading] = useState(false);
  const [response, setResponse] = useState<DeleteJobResponse>();

  const deleteJob = async (request: Omit<DeleteJobRequest, "userToken">) => {
    if (user.token === undefined) {
      notifyErrors({ errors: NOT_LOGGED_IN_ERROR_RESPONSE.errors! });
      return;
    }

    setIsLoading(true);
    setResponse(undefined);

    const result = await jobManagerRestClient.deleteJob({
      ...request,
      userToken: user.token,
    });

    if (result.errors) {
      notifyErrors({ errors: result.errors });
    }

    setIsLoading(false);
    setResponse(result);
  };

  return [deleteJob, isLoading, response];
};
