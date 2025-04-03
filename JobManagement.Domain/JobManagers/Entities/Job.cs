using FluentResults;
using JobManagement.Domain.JobManagers.Entities.Abstractions;
using JobManagement.Domain.JobManagers.Entities.Entities;
using JobManagement.Domain.JobManagers.Entities.Errors;
using JobManagement.Domain.JobManagers.Entities.ValueObjects;
using JobManagement.Domain.JobManagers.Services;

namespace JobManagement.Domain.JobManagers.Entities;

/// <summary>
/// Represents a job task with persistence support
/// </summary>
public class Job
{

    #region Properties

    public static readonly TimeSpan CancelationTimeout = TimeSpan.FromSeconds(10);

    public required JobName Name { get; init; }
    public required JobPriority Priority { get; init; }
    public required JobExecutionName ExecutionName { get; init; }
    public JobStatus Status { get; private set; } = JobStatus.Pending;
    public required DateTime CreatedInUtc { get; init; }
    public required DateTime ExecutionTimeInUtc { get; init; }
    public DateTime? StartTimeInUtc { get; private set; } = null;
    public DateTime? EndTimeInUtc { get; private set; } = null;
    public required JobLog Log { get; init; } // needs to be persisted on db (should be a file on the server and not in db)

    #endregion

    #region Constructors

    private Job() { }

    public static Result<Job> Create(
        JobName name,
        JobExecutionName executionName,
        int priorityValue,
        string? executionTimeInUtcString
        )
    {
        var jobLog = new JobLog();
        var creationTimeInUtc = DateTime.UtcNow; // setting the execution time with creation time if none given
        var executionTimeInUtc = DateTime.UtcNow;

        if (executionTimeInUtcString is not null &&
            DateTime.TryParse(executionTimeInUtcString, out executionTimeInUtc) // overloads executionTimeInUtc if needed
            )
        {
            if (executionTimeInUtc < DateTime.UtcNow)
                return JobsErrorFactory.CannotCreateJobWithExecutionTimeInThePast();
        }

        if (!JobPriority.TryFromValue(priorityValue, out var jobPriority))
            return JobsErrorFactory.JobPriorityIsInvalid();

        jobLog.Append($"job with name - {name} created at time - {creationTimeInUtc.ToString("O")}");

        return new Job
        {
            Name = name,
            Priority = jobPriority,
            ExecutionName = executionName,
            ExecutionTimeInUtc = executionTimeInUtcString is not null ? executionTimeInUtc : creationTimeInUtc,
            CreatedInUtc = creationTimeInUtc,
            Log = jobLog,
        };
    }

    #endregion

    #region Methods

    internal Result<Job> Start(
        IExecutionProvider jobProvider,
        IJobExecutionBag bag)
    {

        var startStatus = Status.Start();

        if (startStatus.IsFailed)
            return Result.Fail(startStatus.Errors);

        var exec = jobProvider.CreateInstance(ExecutionName);

        if (exec is null)
            return JobsErrorFactory.CouldNotFindJobExecution();

        var runExecutable = bag.AppendExecutable(Name, exec);

        if (runExecutable.IsFailed)
        {
            Status = JobStatus.Failed;
            return Result.Fail(runExecutable.Errors);
        }

        exec.Run();

        var startTime = DateTime.UtcNow;

        Log.Append($"job with name - {Name} started at time - {startTime.ToString("O")}");
        Status = startStatus.Value;
        StartTimeInUtc = startTime;

        return Result.Ok(this);
    }

    internal Result<Job> RequestStopJob(
        IJobExecutionBag bag)
    {
        Log.Append($"job with name - {Name} stopping");

        var stopStatus = Status.Stop();

        if (stopStatus.IsFailed)
            return Result.Fail(stopStatus.Errors);

        var execution = bag.GetExecutionByJobName(Name);

        if (execution is null)
            return JobsErrorFactory.CouldNotFindJobExecution();

        execution.Stop();

        Status = stopStatus.Value;

        return Result.Ok(this);
    }

    internal Result<Job> Restart(
        IJobExecutionBag bag)
    {
        var restartStatus = Status.Restart();

        if (restartStatus.IsFailed)
            return Result.Fail(restartStatus.Errors);

        Log.Append($"job with name - {Name} restarting");

        var execution = bag.GetExecutionByJobName(Name);

        if (execution is null)
            return JobsErrorFactory.CouldNotFindJobExecution();

        execution.Stop();

        Status = restartStatus.Value;

        return Result.Ok(this);
    }

    internal Result<Job> Delete()
    {
        var deleteStatus = Status.Delete();

        if (deleteStatus.IsFailed)
            return Result.Fail(deleteStatus.Errors);

        EndTimeInUtc = DateTime.UtcNow;
        Log.Append($"job with name - {Name} deleted");

        return Result.Ok(this);
    }

    public bool ShouldStart(IJobExecutionBag bag)
    {
        if (Status == JobStatus.Pending && ExecutionTimeInUtc < DateTime.UtcNow)
            return true;

        if (Status == JobStatus.Restarting)
        {
            var execution = bag.GetExecutionByJobName(Name);

            if (execution is null)
            {
                Log.Append(
                    $"job with name - {Name} failed. " +
                    $"error - {JobsErrorFactory.CouldNotFindJobExecution().Message}");
                Status = JobStatus.Failed;
                return false;
            }

            if (execution.IsStopped)
                return true;
        }

        return false;
    }

    #endregion

}
