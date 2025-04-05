import { configureStore} from "@reduxjs/toolkit";
import { userReducers } from "./features/user/userSlice";
import { jobsReducers } from "./features/jobs/jobManagerSlice";

const store = configureStore({
  reducer: {
    userSlice: userReducers,
    jobsSlice: jobsReducers,
  }
});

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch;

export default store;