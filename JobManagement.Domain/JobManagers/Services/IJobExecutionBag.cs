using FluentResults;
using JobManagement.Domain.JobManagers.Jobs.Abstractions;
using JobManagement.Domain.JobManagers.Jobs.ValueObjects;

namespace JobManagement.Domain.JobManagers.Services;

/// <summary>
/// Represents the machine in which the exec is running on
/// </summary>
public interface IJobExecutionBag
{
    public Result RemoveExecutable(JobName jobName);
    public Result AppendExecutable(JobName jobName, IJobExecution execution);
    public IJobExecution? GetExecutionByJobName(JobName jobName);
    public IEnumerable<(JobName JobName, IJobExecution Execution)> GetExecutions();

}
