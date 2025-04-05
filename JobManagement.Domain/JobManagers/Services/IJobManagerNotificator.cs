using FluentResults;
using JobManagement.Domain.Common;
using JobManagement.Domain.JobManagers.Entities;

namespace JobManagement.Domain.JobManagers.Services;

public interface IJobManagerNotificationService
{

    public Task<Result> NotifyJobSamplingEnded(
        IEnumerable<Job> jobs,
        IEnumerable<ErrorBase> errors,
        CancellationToken token);

}
