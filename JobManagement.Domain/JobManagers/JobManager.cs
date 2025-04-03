using FluentResults;
using JobManagement.Domain.JobManagers.Entities;
using JobManagement.Domain.JobManagers.Entities.Abstractions;
using JobManagement.Domain.JobManagers.Entities.Errors;
using JobManagement.Domain.JobManagers.Entities.ValueObjects;
using JobManagement.Domain.JobManagers.Services;

namespace JobManagement.Domain.JobManagers;

/// <summary>
/// Represent the db state manager of all jobs and validates steps 
/// with the running processes
/// </summary>
public class JobManager
{

    #region Properties

    public const int MaxConcurrentJobs = 5;


    public int Id { get; init; }
    public List<Job> Jobs { get; private set; } = new List<Job>();

    #endregion

    #region Constructor

    private JobManager() { }


    // Factory method for validation of arguments (which there're not)
    public static Result<JobManager> Create()
    {
        return new JobManager();
    }

    #endregion

    #region Methods

    public Result AppendJob(Job job)
    {

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

    public Result<Job> RequestDeleteJob(JobName jobName)
    {
        var job = FindJobByJobName(jobName);

        if (job.IsFailed)
            return Result.Fail(job.Errors);

        return job.Value.Delete();
    }

    public Result Tick(IJobExecutionBag bag, IExecutionProvider jobProvider)
    {
        var errors = new List<IError>();

        foreach (var job in Jobs)
        {
            if (job.ShouldStart(bag))
            {
                if (Jobs.Count(j => j.Status == JobStatus.Pending) >= MaxConcurrentJobs)
                    break;

                var startResult = job.Start(
                    jobProvider, 
                    bag);

                if (startResult.IsFailed)
                    errors.AddRange(startResult.Errors);

                break;
            }

            if(job.Status == JobStatus.PendingDeletion)
            {
                Jobs.Remove(job);
                break;
            }
        }

        return Result.Ok();
    }

    private Result<Job> FindJobByJobName(JobName jobName)
    {
        var job = Jobs.FirstOrDefault(j => j.Name == jobName);

        if (job is null)
            return JobsErrorFactory.CouldNotFindJob();

        return job;
    }

    #endregion

}
