using JobManagement.Domain.JobManagers.Jobs.ValueObjects;

namespace JobManagement.Domain.JobManagers.Jobs.Abstractions;

/// <summary>
/// Represents all the available executions on remote/local services
/// </summary>
public interface IExecutionProvider
{
    public IEnumerable<string> AvailableJobs { get; }
    IJobExecution? CreateInstance(JobExecutionName name);
}