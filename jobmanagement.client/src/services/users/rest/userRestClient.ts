import { UserClient } from "../contracts/userContracts";
import { api } from "../../commons/api";

export const userRestClient: UserClient = {
  login: async (request) => {
    try {
      const response = await api.post(`/Users/login`, request);

      return await response.data;
    } catch (e) {
      return e;
    }
  },
};
