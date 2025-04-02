using FluentResults;
using JobManagement.Domain.JobManagers.Entities.Entities;
using JobManagement.Domain.JobManagers.Entities.Errors;
using JobManagement.Domain.JobManagers.Entities.ValueObjects;

namespace JobManagement.Domain.JobManagers.Entities;

/// <summary>
/// Represents a job task with persistence support
/// </summary>
public class Job
{
    #region Members

    private readonly IJobExecution _jobExecution; // not persisted
    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _task;

    #endregion

    #region Properties

    public static readonly TimeSpan CancelationTimeout = TimeSpan.FromSeconds(10);

    public required JobName Name { get; init; }
    public required JobPriority Priority { get; init; }
    public JobStatus Status { get; private set; } = JobStatus.Pending;
    public required DateTime CreatedInUtc { get; init; }
    public required DateTime ExecutionTimeInUtc { get; init; }
    public DateTime? StartTimeInUtc { get; private set; } = null;
    public DateTime? EndTimeInUtc { get; private set; } = null;
    public required JobLog Log { get; init; } // needs to be persisted on db (should be a file on the server and not in db)

    #endregion

    #region Constructors

    // only constructible by factory method
#pragma warning disable CS8618 // Non-nullable field (this constructor is for EF core)
    private Job() { }
#pragma warning restore CS8618

    private Job(IJobExecution jobExecution)
    {
        _jobExecution = jobExecution;
    }

    public static Result<Job> Create(
        JobName name,
        JobPriority priority,
        DateTime? executionTimeInUtc,
        IJobExecution jobExecution
        )
    {
        if (executionTimeInUtc < DateTime.UtcNow)
            return JobsErrorFactory.CannotCreateJobWithExecutionTimeInThePast();

        var creationTime = DateTime.UtcNow; // setting the execution time with creation time if none given

        var jobLog = new JobLog();
        jobLog.Append($"job with name - {name} created at time - {creationTime.ToString("O")}");

        return new Job(jobExecution)
        {
            Name = name,
            Priority = priority,
            ExecutionTimeInUtc = executionTimeInUtc ?? creationTime,
            CreatedInUtc = creationTime,
            Log = jobLog,
        };
    }

    #endregion


    #region Methods

    public Result Start()
    {
        if (ExecutionTimeInUtc > DateTime.UtcNow)
            return JobsErrorFactory.CannotStartJobWithExecutionTimeInTheFuture();

        var startStatus = Status.Start();

        if (startStatus.IsFailed)
            return Result.Fail(startStatus.Errors);

        Status = startStatus.Value;

        var startTime = DateTime.UtcNow;

        Log.Append($"job with name - {Name} started at time - {startTime.ToString("O")}");
        StartTimeInUtc = startTime;

        _cancellationTokenSource = new();
        _task = _jobExecution.Run(_cancellationTokenSource.Token);

        return Result.Ok();
    }

    public Result RequestStopJob()
    {

        Log.Append($"job with name - {Name} stopping");

        var stopStatus = Status.Stop();

        if (stopStatus.IsFailed)
            return Result.Fail(stopStatus.Errors);

        Status = stopStatus.Value;

        try
        {
            _cancellationTokenSource!.Cancel(); // should not be null as job must be running state
            _task!.Wait(CancelationTimeout); // should be canceled by cancellation token 
        }
        catch (Exception)
        {
            Log.Append($"job with name - {Name} failed to stop. stopping forcefully");
            _jobExecution.ForceStop();

            return JobsErrorFactory.ExecutionJobFailedToStop();
        }

        return Result.Ok();
    }

    public Result<Job> Restart()
    {
        var restartStatus = Status.Restart();

        if (restartStatus.IsFailed)
            return Result.Fail(restartStatus.Errors);

        Log.Append($"job with name - {Name} restarting");

        RequestStopJob();

        Status = restartStatus.Value;

        Start();

        return Result.Ok();
    }

    public Result Delete()
    {
        var deleteStatus = Status.Delete();

        if (deleteStatus.IsFailed)
            return Result.Fail(deleteStatus.Errors);

        Log.Append($"job with name - {Name} deleted");

        return Result.Ok();
    }

    #endregion

}
