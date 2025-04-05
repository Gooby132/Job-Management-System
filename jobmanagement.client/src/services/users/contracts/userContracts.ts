import { GenericResponse } from "../../commons/contracts";

export type UserClient = {
  login: (request: LoginRequest) => Promise<LoginResponse>;
}

// login

export type LoginRequest = {
  userName: string;
  password: string;
}

export type LoginResponse = {
  token?: string;
  userName?: string;
} & GenericResponse;