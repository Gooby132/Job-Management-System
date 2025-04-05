import * as signalR from "@microsoft/signalr";
import { JOB_MANAGER_API_BASE_URL } from "../../commons/api";
import { JobsStatusesResponse } from "../contracts/jobManagerContracts";
import store from "../../../redux/store";
import { jobsActions } from "../../../redux/features/jobs/jobManagerSlice";

const dispatch = store.dispatch;

export const startSignalRConnection = async () => {
  const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${JOB_MANAGER_API_BASE_URL}/JobManagerHub`,{
      withCredentials: false,
    })
    .withAutomaticReconnect()
    .build();

  connection.on("JobStatusChanged", (response: JobsStatusesResponse) => {
    dispatch(jobsActions.setJobStatuses(response));
  });

  await connection.start()
};
