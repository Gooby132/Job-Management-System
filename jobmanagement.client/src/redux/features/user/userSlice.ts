import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { LoginResponse } from "../../../services/users/contracts/userContracts";

type UserState = {
  userName?: string;
  token?: string;
};

const initialState: UserState = {
  userName: undefined,
  token: undefined,
};

export const user = createSlice({
  name: "user",
  initialState: initialState,
  reducers: {
    loginUser: (state, action: PayloadAction<LoginResponse>) => {
      state.token = action.payload.token;
      state.userName = action.payload.userName;
    },
    logoutUser: (state) => {
      state = initialState;
    },
  },
});

export const userReducers = user.reducer;
export const userActions = user.actions;