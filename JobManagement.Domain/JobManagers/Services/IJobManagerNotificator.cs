using FluentResults;
using JobManagement.Domain.Common;
using JobManagement.Domain.JobManagers.Entities;

namespace JobManagement.Domain.JobManagers.Services;

/// <summary>
/// Represents the service that notifies when a sample was made
/// </summary>
public interface IJobManagerNotificationService
{

    public Task<Result> NotifyJobSamplingEnded(
        IEnumerable<Job> jobs,
        IEnumerable<ErrorBase> errors,
        CancellationToken token);

}
