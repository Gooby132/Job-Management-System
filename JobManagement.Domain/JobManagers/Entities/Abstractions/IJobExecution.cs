using FluentResults;
using JobManagement.Domain.JobManagers.Entities.ValueObjects;

namespace JobManagement.Domain.JobManagers.Entities.Abstractions;

/// <summary>
/// Should represent an execution for a job
/// </summary>
public interface IJobExecution
{

    /// <summary>
    /// Represents the name of the job
    /// </summary>
    public static JobExecutionName Name { get; }

    /// <summary>
    /// Represents the percentage of the progress 0 - 100
    /// </summary>
    public int Progress { get; }

    /// <summary>
    /// Represents if the execution stopped
    /// </summary>
    public bool IsStopped { get; }

    /// <summary>
    /// Method for starting execution 
    /// </summary>
    /// <returns>The result of the process</returns>
    public void Run();

    /// <summary>
    /// Forces the execution to stop (could be by os)
    /// </summary>
    /// <returns></returns>
    public void ForceStop();

    /// <summary>
    /// Gracefully stops the execution
    /// </summary>
    public void Stop();

}
