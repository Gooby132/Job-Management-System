import { useState } from "react";
import {
  LoginRequest,
  LoginResponse,
} from "../services/users/contracts/userContracts";
import { userRestClient } from "../services/users/rest/userRestClient";
import { useDispatch } from "react-redux";
import { notifyErrors } from "../services/helpers/notifier";
import { userActions } from "../redux/features/user/userSlice";

export const useLogin = (): [
  (request: LoginRequest) => Promise<void>,
  boolean,
  LoginResponse | undefined
] => {
  const dispatch = useDispatch();
  const [isLoading, setIsLoading] = useState(false);
  const [response, setResponse] = useState<LoginResponse>();

  const login = async (request: LoginRequest) => {
    setIsLoading(true);
    setResponse(undefined);

    const result = await userRestClient.login(request);

    if(result.errors)
      notifyErrors({ errors: result.errors });

    if(result.token)
      dispatch(userActions.loginUser(result))

    setIsLoading(false);
    setResponse(result);
  };

  return [login, isLoading, response];
};
