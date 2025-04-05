using Ardalis.SmartEnum;
using FluentResults;
using JobManagement.Domain.JobManagers.Entities.Errors;

namespace JobManagement.Domain.JobManagers.Entities.ValueObjects;

/// <summary>
/// Represents the state machine of the statuses of a job execution
/// </summary>
public class JobStatus : SmartEnum<JobStatus>
{

    public static readonly JobStatus Pending = new JobStatus(nameof(Pending), 1);
    public static readonly JobStatus Running = new JobStatus(nameof(Running), 2);
    public static readonly JobStatus Completed = new JobStatus(nameof(Completed), 3);
    public static readonly JobStatus Failed = new JobStatus(nameof(Failed), 4);
    public static readonly JobStatus Canceled = new JobStatus(nameof(Canceled), 5);
    public static readonly JobStatus Stopping = new JobStatus(nameof(Stopping), 6);
    public static readonly JobStatus Restarting = new JobStatus(nameof(Restarting), 7);
    public static readonly JobStatus Stopped = new JobStatus(nameof(Stopped), 8);
    public static readonly JobStatus PendingDeletion = new JobStatus(nameof(PendingDeletion), 9);

    private JobStatus(string name, int value) :
        base(name, value)
    { }

    public bool ShouldSample() =>
        this == Pending ||
        this == Restarting ||
        this == Running ||
        this == Stopping;

    public Result<JobStatus> Start()
    {
        if (!(this != Pending || this != Restarting))
            return JobsErrorFactory.CannotStartJobNotInPendingOrRestartingState();

        return Running;
    }

    public Result<JobStatus> Stop()
    {
        if (this != Running)
            return JobsErrorFactory.CannotStopJobWhichDidNotStart();

        return Stopping;
    }

    public Result<JobStatus> Restart()
    {
        if (this == Pending)
            return JobsErrorFactory.CannotRestartJobWhichWasNotStarted();

        return Restarting;
    }

    public Result<JobStatus> Delete()
    {
        if (!(this != Completed ||
            this != Failed))
            return JobsErrorFactory.CannotDeleteJobNotInCompletedOrFailedStatus();

        return PendingDeletion;
    }

    public Result<JobStatus> HasCompleted()
    {
        if (!(
            this != Pending ||
            this != Running ||
            this != Restarting)) // running and pending states if sampling interval was too large
            return JobsErrorFactory.CannotCompleteJobIfWasNotPending();

        if (this == Stopping)
            return Stopped;

        return Completed;
    }

    public Result<JobStatus> DidFail() 
    {
        if(this == Pending)
            return Result.Fail(JobsErrorFactory.CannotFailJobStatusWhichDidNotRun());

        if (this == Completed)
            return Result.Fail(JobsErrorFactory.CannotFailJobStatusThatAlreadyCompleted());

        if (this == Stopped)
            return Result.Fail(JobsErrorFactory.CannotFailJobStatusThatAlreadyCompleted());

        return Failed;
    }

}
