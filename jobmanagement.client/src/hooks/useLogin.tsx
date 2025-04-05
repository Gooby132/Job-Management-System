import { useState } from "react";
import {
  LoginRequest,
  LoginResponse,
} from "../services/jobManager/contracts/jobManagerContracts";
import { jobManagerRestClient } from "../services/jobManager/rest/jobManagerRestClient";

export const useLogin = (): [
  (request: LoginRequest) => Promise<void>,
  boolean,
  LoginResponse | undefined
] => {
  const [isLoading, setIsLoading] = useState(false);
  const [response, setResponse] = useState<LoginResponse>();

  const login = async (request: LoginRequest) => {
    setIsLoading(true);
    setResponse(undefined);

    const result = await jobManagerRestClient.login(request);

    setIsLoading(false);
    setResponse(result);
  };

  return [login, isLoading, response];
};
