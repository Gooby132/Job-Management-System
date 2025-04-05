export const jobManagerRules = {
  job: {
    jobName: (value: string) => {
      if (value.length < 2) {
        return "Job name must be at least 2 characters long";
      }
      if (value.length > 50) {
        return "Job name must be at most 50 characters long";
      }
      return null;
    },
    jobPriority: (value: number) => {
      if (value !== 1 && value !== 2) {
        return "Job priority is invalid";
      }

      return null;
    },
    executionName: (value: string) => {
      if (value.length < 2) {
        return "Execution name must be at least 2 characters long";
      }

      if (value.length > 50) {
        return "Execution name must be at most 50 characters long";
      }

      return null;
    },
    executionTime: (value: string) => {
      if(value.length === 0) // no need to set execution time
        return null

      const date = new Date(value);

      if (isNaN(date.getTime())) {
        return "Execution time is invalid";
      }

      if (date.getTime() < Date.now()) {
        return "Execution time must be in the future";
      }

      return null;
    },
  },
};
