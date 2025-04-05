import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { JobDto, JobsStatusesResponse } from "../../../services/jobManager/contracts/jobManagerContracts";

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
    }
  },
});

export const jobsReducers = jobs.reducer;
export const jobsActions = jobs.actions;