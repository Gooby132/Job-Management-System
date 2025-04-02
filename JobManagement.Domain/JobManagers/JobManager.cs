using FluentResults;
using JobManagement.Domain.JobManagers.Entities;
using JobManagement.Domain.JobManagers.Entities.ValueObjects;

namespace JobManagement.Domain.JobManagers;

/// <summary>
/// Represent the manager of all jobs
/// </summary>
public class JobManager
{

    public List<Job> Jobs { get; private set; } = new List<Job>();

    private JobManager() { }

    public Result AppendJob(Job job)
    {

        // if no jobs
        // if 

        if (job.Priority == JobPriority.Regular)
        {
            Jobs.Add(job);
            return Result.Ok();
        }

        int firstRegularPriority;
        for (int i = 0; i < Jobs.Count - 1; i++)
        {
            if (Jobs[i].Priority == JobPriority.Regular)
            {
                firstRegularPriority = i;
                break;
            }
        }

        return Result.Ok();
    }

}
