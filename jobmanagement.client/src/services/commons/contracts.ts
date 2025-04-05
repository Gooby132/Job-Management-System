export const UNKNOWN_ERROR_RESPONSE: GenericResponse = {
  errors: [{ errorCode: 0, groupCode: 0, message: "Unknown error" }],
};

export const NOT_LOGGED_IN_ERROR_RESPONSE: GenericResponse = {
  errors: [{ errorCode: 0, groupCode: 0, message: "Not logged in" }],
};

export type GenericResponse = {
  errors?: ErrorDto[];
};

export type ErrorDto = {
  groupCode: number;
  errorCode: number;
  message: string;
};
