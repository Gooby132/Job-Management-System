export const UNKNOWN_ERROR_RESPONSE: GenericResponse = {
  errors: [{ errorCode: 0, groupCode: 0, message: "Unknown error" }],
};

export type GenericResponse = {
  errors?: ErrorDto[];
};

export type ErrorDto = {
  groupCode: number;
  errorCode: number;
  message: string;
};
