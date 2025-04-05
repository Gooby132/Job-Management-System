import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { AppendJobResponse, JobDto, JobsStatusesResponse } from "../../../services/jobManager/contracts/jobManagerContracts";

type JobsState = {
  jobs: JobDto[];
};

const initialState: JobsState = {
  jobs: [],
};

export const jobs = createSlice({
  name: "jobs",
  initialState: initialState,
  reducers: {
    setJobStatuses: (state, action: PayloadAction<JobsStatusesResponse>) => {
      state.jobs = action.payload.jobs ?? [];
    },
    appendJob: (state, action: PayloadAction<AppendJobResponse>) => {
      state.jobs = action.payload.job ? [...state.jobs, action.payload.job] : state.jobs;
    }
  },
});

export const jobsReducers = jobs.reducer;
export const jobsActions = jobs.actions;