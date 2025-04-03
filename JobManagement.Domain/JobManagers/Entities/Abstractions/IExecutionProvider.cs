using JobManagement.Domain.JobManagers.Entities.ValueObjects;

namespace JobManagement.Domain.JobManagers.Entities.Abstractions;

public interface IExecutionProvider
{
    public IEnumerable<string> AvailableJobs { get; }
    IJobExecution? CreateInstance(JobExecutionName name);
}