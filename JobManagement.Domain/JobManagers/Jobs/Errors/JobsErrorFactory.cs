using FluentResults;
using JobManagement.Domain.Common;

namespace JobManagement.Domain.JobManagers.Jobs.Errors;

/// <summary>
/// Represents all errors avaialable by the job manager
/// </summary>
public static class JobsErrorFactory
{
    public const int JobsGroupCode = 1;

    public static ErrorBase CannotCreateJobWithExecutionTimeInThePast() =>
        new ErrorBase(JobsGroupCode, 1, "cannot create job with start time in the past");

    public static ErrorBase CannotStopJobWhichHasAlreadyBeenStopped() =>
        new ErrorBase(JobsGroupCode, 2, "cannot stop job which has already been stopped");

    public static ErrorBase CannotDeleteJobNotInCompletedOrFailedStatus() =>
        new ErrorBase(JobsGroupCode, 3, "cannot delete job not in completed or failed status");

    public static ErrorBase CannotStartJobWithExecutionTimeInTheFuture() =>
        new ErrorBase(JobsGroupCode, 4, "cannot start job with start time in the future");

    public static ErrorBase CannotStartJobNotInPendingOrRestartingState() =>
        new ErrorBase(JobsGroupCode, 5, "cannot start job not in pending status");

    public static ErrorBase ExecutionJobFailedToStop() =>
        new ErrorBase(JobsGroupCode, 6, "execution job failed to stop");

    public static ErrorBase CannotStopJobWhichDidNotStart() =>
        new ErrorBase(JobsGroupCode, 7, "cannot stop job which did not start or already finished");

    public static ErrorBase JobRestartIsAllowedOnlyOnStoppedOrFailedStatuses() =>
        new ErrorBase(JobsGroupCode, 8, "job restart is allowed only on stopped or failed statuses");

    public static ErrorBase CannotAppendJobAsMaxConcurrentJobsReached() =>
        new ErrorBase(JobsGroupCode, 9, "cannot append job as max concurrent jobs reached");

    public static ErrorBase CouldNotFindJob() =>
        new ErrorBase(JobsGroupCode, 10, "could not find job");

    public static ErrorBase CouldNotFetchJobManagerFromRepository() =>
        new ErrorBase(JobsGroupCode, 11, "could not fetch job manager from repository");

    public static ErrorBase FailedUpdatingJobManager() =>
        new ErrorBase(JobsGroupCode, 12, "failed updating job manager");

    public static ErrorBase FailedJobManagerToRepository() =>
        new ErrorBase(JobsGroupCode, 13, "failed job manager to repository");

    public static ErrorBase JobNameIsInvalid() =>
        new ErrorBase(JobsGroupCode, 14, "job name is invalid");

    public static ErrorBase JobNameIsTooShort() =>
        new ErrorBase(JobsGroupCode, 15, "job name is too short");

    public static ErrorBase JobNameIsTooLong() =>
        new ErrorBase(JobsGroupCode, 16, "job name is too long");

    public static ErrorBase JobPriorityIsInvalid() =>
        new ErrorBase(JobsGroupCode, 17, "job priority is invalid");

    public static ErrorBase JobCanceled() =>
        new ErrorBase(JobsGroupCode, 18, "job was canceled");

    public static ErrorBase JobExecutionNameIsInvalid() =>
    new ErrorBase(JobsGroupCode, 19, "job exec name is invalid");

    public static ErrorBase JobExecutionNameIsTooShort() =>
        new ErrorBase(JobsGroupCode, 20, "job exec name is too short");

    public static ErrorBase JobExecutionNameIsTooLong() =>
        new ErrorBase(JobsGroupCode, 21, "job exec name is too long");

    public static ErrorBase CouldNotAttachJobExecutable() =>
        new ErrorBase(JobsGroupCode, 22, "could not attach job executable");

    public static ErrorBase CouldNotFindJobExecution() =>
        new ErrorBase(JobsGroupCode, 23, "could not find job execution");

    public static ErrorBase JobExecutionIsAlreadyRunning() =>
        new ErrorBase(JobsGroupCode, 24, "job execution is already running");

    public static ErrorBase CouldNotRemoveJobExecution() =>
        new ErrorBase(JobsGroupCode, 25, "could not remove job execution");

    public static ErrorBase CannotCompleteJobIfWasNotPending() =>
        new ErrorBase(JobsGroupCode, 26, "cannot complete job if was not pending");

    public static ErrorBase JobWasNotFound() =>
        new ErrorBase(JobsGroupCode, 27, "job was not found");

    public static ErrorBase NotifyJobSamplingFailed() =>
        new ErrorBase(JobsGroupCode, 28, "notify job sampling failed");

    public static ErrorBase CannotFailJobStatusThatAlreadyCompleted() =>
        new ErrorBase(JobsGroupCode, 29, "cannot fail job status that already completed");

    public static ErrorBase CannotFailJobStatusWhichDidNotRun() =>
        new ErrorBase(JobsGroupCode, 30, "cannot fail job status which did not run");

    public static ErrorBase ExecutionJobFailed() =>
        new ErrorBase(JobsGroupCode, 31, "execution job failed");

    public static Result JobWithTheSameNameAlreadyExists() => 
        new ErrorBase(JobsGroupCode, 32, "a job with the same name already exists");

}
