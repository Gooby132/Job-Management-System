import axios, { HttpStatusCode } from "axios";
import { GenericResponse, UNKNOWN_ERROR_RESPONSE } from "./contracts";
import axiosRetry from 'axios-retry';

export const JOB_MANAGER_API_BASE_URL =
  import.meta.env.VITE_JOB_MANAGER_API_BASE_URL || "http://localhost:5287/api";

axiosRetry(axios, { retryDelay: axiosRetry.exponentialDelay });

export const api = axios.create({
  baseURL: JOB_MANAGER_API_BASE_URL,
  withCredentials: false,
});

api.interceptors.response.use(null, (error): Promise<GenericResponse> => {
  if (!axios.isAxiosError(error)) return Promise.reject(UNKNOWN_ERROR_RESPONSE);

  if (error.status === HttpStatusCode.BadRequest) {
    return Promise.reject({
      errors: error.response?.data?.errors || [],
    });
  }

  if (error.status === HttpStatusCode.NotFound) {
    return Promise.reject({
      errors: error.response?.data?.errors || [],
    });
  }

  if (error.status === HttpStatusCode.UnprocessableEntity) {
    return Promise.reject({
      errors: error.response?.data?.errors || [],
    });
  }

  return Promise.reject(UNKNOWN_ERROR_RESPONSE);
});
