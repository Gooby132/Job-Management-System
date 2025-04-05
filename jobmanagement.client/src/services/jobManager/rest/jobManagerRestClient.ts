import { api } from "../../commons/api";
import { JobManagerClient } from "../contracts/jobManagerContracts";

export const jobManagerRestClient: JobManagerClient = {
  getJob: async (request) => {
    try {
      const response = await api.get(`/JobManager/get-job`, {
        params: request,
      });

      return await response.data;
    }
    catch (e) {
      return e;
    }
  },
  appendJob: async (request) => {
    try {
      const response = await api.post(`/JobManager/append-job`, request);

      return await response.data;
    } catch (e) {
      return e;
    }
  },
  getAvailableJobsExecutions: async (request) => {
    try {
      const response = await api.get(
        `/JobManager/available-job-executions`,
        request
      );

      return await response.data;
    } catch (e) {
      return e;
    }
  },
  deleteJob: async (request) => {
    try {
      const response = await api.post(`/JobManager/delete-job`, request,{
        headers: {
          Authorization: `Bearer ${request.userToken}`,
        },
      });

      return await response.data;
    } catch (e) {
      return e;
    }
  },
  jobsStatuses: async (request) => {
    try {
      const response = await api.get(`/JobManager/jobs-statuses`, request);

      return await response.data;
    } catch (e) {
      return e;
    }
  },
  requestStopJob: async (request) => {
    try {
      const response = await api.post(`/JobManager/request-stop-job`, request);
      return await response.data;
    } catch (e) {
      return e;
    }
  },
  restartJob: async (request) => {
    try {
      const response = await api.post(`/JobManager/restart-job`, request);

      return await response.data;
    } catch (e) {
      return e;
    }
  },
  startJob: async (request) => {
    try {
      const response = await api.post(`/JobManager/start-job`, request);
      return await response.data;
    } catch (e) {
      return e;
    }
  },
};
