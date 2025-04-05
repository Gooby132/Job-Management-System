using FluentResults;
using JobManagement.Domain.Common;
using JobManagement.Domain.JobManagers.Jobs;
using JobManagement.Domain.JobManagers.Jobs.Abstractions;
using JobManagement.Domain.JobManagers.Jobs.Errors;
using JobManagement.Domain.JobManagers.Jobs.ValueObjects;
using JobManagement.Domain.JobManagers.Services;

namespace JobManagement.Domain.JobManagers;

/// <summary>
/// Represent the db state manager of all jobs and validates steps 
/// with the running processes
/// this is the AR and the underlying entities (jobs) should be handled by this class only
/// </summary>
public class JobManager : IAggregateRoot
{

    #region Properties

    public const int MaxConcurrentJobs = 5;

    public int Id { get; init; }
    public List<Job> Jobs { get; private set; } = new List<Job>();

    #endregion

    #region Constructor

    private JobManager() { }


    /// <summary>
    /// Factory method for validation of arguments (which there're not)
    /// </summary>
    /// <returns></returns>
    public static Result<JobManager> Create()
    {
        return new JobManager();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Appends job into the correct position based on priority
    /// </summary>
    /// <param name="job"></param>
    /// <returns></returns>
    public Result AppendJob(Job job)
    {
        if (GetJobByJobName(job.Name) != null)
            return JobsErrorFactory.JobWithTheSameNameAlreadyExists();

        if (job.Priority == JobPriority.Regular)
        {
            Jobs.Add(job);
            return Result.Ok();
        }

        int firstRegularPriority = 0;
        for (int i = 0; i < Jobs.Count - 1; i++)
        {
            if (Jobs[i].Priority == JobPriority.Regular)
            {
                firstRegularPriority = i;
                break;
            }
        }

        Jobs.Insert(firstRegularPriority, job);

        return Result.Ok();
    }

    public Result<Job> StartJob(JobName jobName, IExecutionProvider jobProvider, IJobExecutionBag bag)
    {
        var job = FindJobByJobName(jobName);

        if (job.IsFailed)
            return Result.Fail(job.Errors);

        return job.Value.Start(jobProvider, bag);
    }

    public Result<Job> RequestStopJob(JobName jobName, IJobExecutionBag bag)
    {
        var job = FindJobByJobName(jobName);

        if (job.IsFailed)
            return Result.Fail(job.Errors);

        return job.Value.RequestStopJob(bag);
    }

    public Result<Job> RestartJob(JobName jobName, IJobExecutionBag bag)
    {
        var job = FindJobByJobName(jobName);

        if (job.IsFailed)
            return Result.Fail(job.Errors);

        return job.Value.Restart(bag);
    }

    public Result<Job> RequestDeleteJob(JobName jobName, IJobExecutionBag bag)
    {
        var job = FindJobByJobName(jobName);

        if (job.IsFailed)
            return Result.Fail(job.Errors);

        return job.Value.Delete(bag);
    }

    /// <summary>
    /// Iterates over the jobs and sample their statuses
    /// </summary>
    /// <param name="bag">executions</param>
    /// <param name="jobProvider">execution factory</param>
    /// <param name="notificationService">jobs sampled notification</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<Result> Tick(
        IJobExecutionBag bag,
        IExecutionProvider jobProvider,
        IJobManagerNotificationService? notificationService,
        CancellationToken token = default)
    {
        var errors = new List<IError>();

        foreach (var job in Jobs)
        {
            if (job.ShouldStart(bag))
            {
                if (IsMaxConcurrentJobsReached())
                    continue;

                var startResult = job.Start(
                    jobProvider,
                    bag);

                if (startResult.IsFailed)
                    errors.AddRange(startResult.Errors);

                continue;
            }

            var sample = job.Sample(bag);

            if (sample.IsFailed)
                errors.AddRange(sample.Errors);

            if (job.Status == JobStatus.PendingDeletion)
            {
                Jobs.Remove(job);
                break;
            }
        }

        if (notificationService != null)
            await notificationService.NotifyJobSamplingEnded(
                Jobs,
                errors
                    .Where(e => e is ErrorBase)
                    .Select(e => (ErrorBase)e),
                token);

        if (errors.Any())
            return Result.Fail(errors);

        return Result.Ok();
    }

    public Job? GetJobByJobName(JobName jobName) => Jobs.FirstOrDefault(j => j.Name == jobName);

    public bool IsMaxConcurrentJobsReached() =>
        Jobs.Count(j => j.Status == JobStatus.Pending) >= MaxConcurrentJobs;

    #endregion

    #region Supports

    private Result<Job> FindJobByJobName(JobName jobName)
    {
        var job = Jobs.FirstOrDefault(j => j.Name == jobName);

        if (job is null)
            return JobsErrorFactory.CouldNotFindJob();

        return job;
    }

    #endregion

}
