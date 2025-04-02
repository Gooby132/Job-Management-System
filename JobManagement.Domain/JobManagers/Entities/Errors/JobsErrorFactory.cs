using FluentResults;
using JobManagement.Domain.Common;

namespace JobManagement.Domain.JobManagers.Entities.Errors;

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

    public static Result ExecutionJobFailedToStop() =>
        new ErrorBase(JobsGroupCode, 6, "execution job failed to stop");

    public static Result CannotStopJobWhichDidNotStart() =>
        new ErrorBase(JobsGroupCode, 7, "cannot stop job which did not start");

    public static Result CannotRestartJobWhichWasNotStarted() =>
        new ErrorBase(JobsGroupCode, 8, "cannot restart job which was not started");

}
