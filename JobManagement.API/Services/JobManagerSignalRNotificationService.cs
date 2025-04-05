using FluentResults;
using JobManagement.API.Contracts.JobManager.Responses;
using JobManagement.API.Helpers;
using JobManagement.API.Hubs;
using JobManagement.Domain.Common;
using JobManagement.Domain.JobManagers;
using JobManagement.Domain.JobManagers.Entities;
using JobManagement.Domain.JobManagers.Entities.Abstractions;
using JobManagement.Domain.JobManagers.Entities.Errors;
using JobManagement.Domain.JobManagers.Services;
using Microsoft.AspNetCore.SignalR;

namespace JobManagement.API.Services;

/// <summary>
/// Represents the concrete domain service 
/// in which the clients receives live statuses about the jobs
/// </summary>
public class JobManagerSignalRNotificationService : IJobManagerNotificationService
{
    #region Fields

    private readonly ILogger<JobManagerSignalRNotificationService> _logger;
    private readonly IHubContext<JobManagerHub> _hubContext;
    private readonly IJobExecutionBag _executionBag;

    #endregion

    #region Contructor

    public JobManagerSignalRNotificationService(
        ILogger<JobManagerSignalRNotificationService> logger,
        IHubContext<JobManagerHub> hubContext,
        IJobExecutionBag executionBag)
    {
        _logger = logger;
        _hubContext = hubContext;
        _executionBag = executionBag;
    }

    public async Task<Result> NotifyJobSamplingEnded(
        IEnumerable<Job> jobs,
        IEnumerable<ErrorBase> errors,
        CancellationToken token)
    {

        try
        {
            var executions = _executionBag.GetExecutions();
            var joinedJobAndExec = jobs
                .GroupJoin(executions, j => j.Name, e => e.JobName, (job, exec) =>
                    new
                    {
                        job,
                        exec
                    })
                .SelectMany(
                    je => je.exec.DefaultIfEmpty(),
                    (je, exec) => new KeyValuePair<Job, IJobExecution>(
                        je.job,
                        exec.Execution
                )).ToList();

            await _hubContext.Clients.All
                .SendAsync(
                    JobManagerHub.JobStatusChangeMethodName,
                    new JobsStatusesResponse
                    {
                        Errors = errors.ToDtos(),
                        Jobs = joinedJobAndExec.ToDtos(),
                    },
                    token);

            _logger.LogDebug("{this} notified all client about job statuses",
                this);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(
                JobsErrorFactory.NotifyJobSamplingFailed().CausedBy(e));
        }
    }

    public override string ToString() => nameof(JobManagerSignalRNotificationService);

    #endregion

}
