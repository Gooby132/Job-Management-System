using FluentResults;
using JobManagement.Domain.JobManagers.Entities.Abstractions;
using JobManagement.Domain.JobManagers.Entities.Errors;
using JobManagement.Domain.JobManagers.Entities.ValueObjects;
using JobManagement.Domain.JobManagers.Services;
using System.Collections.Concurrent;

namespace JobManagement.Infrastructure.ConcreteJobs.InMemoryJobExecutionBag;

public class JobExecutionBag : IJobExecutionBag
{

    private IDictionary<JobName, IJobExecution> _jobExecutions =
        new ConcurrentDictionary<JobName, IJobExecution>();

    public Result AppendExecutable(JobName jobName, IJobExecution execution) =>
        _jobExecutions.TryAdd(jobName, execution) ? Result.Ok() : Result.Fail(JobsErrorFactory.CouldNotAttachJobExecutable());

    public IJobExecution? GetExecutionByJobName(JobName jobName) =>
        _jobExecutions.TryGetValue(jobName, out var execution) ?
            execution : 
            null;

    public IEnumerable<(JobName JobName, IJobExecution Execution)> GetExecutions() =>
        _jobExecutions
            .Select(kv => (kv.Key, kv.Value));

    public Result RemoveExecutable(JobName jobName) =>
        _jobExecutions.Remove(jobName) ? Result.Ok() : Result.Fail(JobsErrorFactory.CouldNotRemoveJobExecution());
}
