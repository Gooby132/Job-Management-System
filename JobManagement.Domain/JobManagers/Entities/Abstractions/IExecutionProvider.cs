using JobManagement.Domain.JobManagers.Entities.ValueObjects;

namespace JobManagement.Domain.JobManagers.Entities.Abstractions;

/// <summary>
/// Represents all the available executions on remote/local services
/// </summary>
public interface IExecutionProvider
{
    public IEnumerable<string> AvailableJobs { get; }
    IJobExecution? CreateInstance(JobExecutionName name);
}