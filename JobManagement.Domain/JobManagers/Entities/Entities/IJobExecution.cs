using FluentResults;

namespace JobManagement.Domain.JobManagers.Entities.Entities;

/// <summary>
/// Should represent an execution for a job
/// </summary>
public interface IJobExecution
{

    /// <summary>
    /// Represents the percentage of the progress 0 - 100
    /// </summary>
    public int Progress { get; }

    /// <summary>
    /// Method for starting execution 
    /// </summary>
    /// <param name="token">cancellation token for canceling the job</param>
    /// <returns>The result of the process</returns>
    public Task<Result> Run(CancellationToken token);

    /// <summary>
    /// Forces the execution to stop (could be by os)
    /// </summary>
    /// <returns></returns>
    public void ForceStop();

}
