using FluentResults;
using JobManagement.Domain.Common;
using JobManagement.Domain.JobManagers;
using JobManagement.Domain.JobManagers.Jobs.Abstractions;
using JobManagement.Domain.JobManagers.Services;
using JobManagement.Infrastructure.JobBackgroundService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Represents a background service
/// that fetches the job manager from the db
/// allows it to sample the running executions
/// and persists the values back to db
/// </summary>
public class JobManagerService : BackgroundService
{
    #region Fields

    private readonly ILogger<JobManagerService> _logger;
    private readonly IServiceScopeFactory _factory;
    private readonly JobServiceOptions _options;
    private readonly IExecutionProvider _jobProvider;
    private readonly IJobExecutionBag _jobExecutionBag;
    private readonly IJobManagerNotificationService _notificationService;

    #endregion

    #region Properties

    public TimeSpan ServiceInvocationIntervalInSeconds { get; }

    #endregion

    #region Constructor
    public JobManagerService(
        ILogger<JobManagerService> logger,
        IServiceScopeFactory factory,
        IExecutionProvider jobProvider,
        IJobExecutionBag jobExecutionBag,
        IJobManagerNotificationService notificationService,
        IOptions<JobServiceOptions> options)
    {
        _logger = logger;
        _factory = factory;
        _jobProvider = jobProvider;
        _jobExecutionBag = jobExecutionBag;
        _notificationService = notificationService;
        _options = options.Value;

        ServiceInvocationIntervalInSeconds = TimeSpan.FromSeconds(_options.ServiceInvocationIntervalInSeconds);
    }

    #endregion

    #region Execution

    /// <summary>
    /// Fetches the job manager and make it sample it's executions
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{this} started", this);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _factory.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var repository = scope.ServiceProvider.GetRequiredService<IJobManagerRepository>();

            _logger.LogDebug("{this} iteration started", this);

            var jobManager = await FetchOrCreateJobManager(repository, uow, stoppingToken);

            if (jobManager.IsFailed)
            {
                _logger.LogCritical("{this} updating job manager failed. errors - '{errors}'",
                    this, string.Join(", ", jobManager.Errors.Select(e => e.Message)));

                break;
            }

            var tickResult = await jobManager.Value.Tick(
                _jobExecutionBag,
                _jobProvider,
                _notificationService,
                stoppingToken);

            if (tickResult.IsFailed)
            {
                _logger.LogWarning("{this} job manager tick failed. errors - {errors}",
                    this, string.Join(", ", tickResult.Errors.Select(e => e.Message)));
            }

            var update = repository.Update(jobManager.Value, stoppingToken);

            if (update.IsFailed)
            {
                _logger.LogCritical("{this} updating job manager failed. errors - '{errors}'",
                    this, string.Join(", ", update.Errors.Select(e => e.Message)));

                await uow.Rollback();

                continue;
            }

            var commit = await uow.Commit(stoppingToken);

            if (commit.IsFailed)
            {
                _logger.LogCritical("{this} committing changes failed. errors - '{errors}'",
                    this, string.Join(", ", commit.Errors.Select(e => e.Message)));
            }

            _logger.LogDebug("{this} iteration ended", this);

            await Task.Delay(ServiceInvocationIntervalInSeconds, stoppingToken);
        }

        _logger.LogInformation("{this} stopping", this);
    }

    #endregion

    #region Supports

    private async Task<Result<JobManager>> FetchOrCreateJobManager(
        IJobManagerRepository repository,
        IUnitOfWork uow,
        CancellationToken token)
    {
        var jobManagerResult = await repository.GetJobManager(token);

        if (jobManagerResult.IsFailed)
            return Result.Fail(jobManagerResult.Errors);

        if (jobManagerResult.Value is not null)
            return Result.Ok(jobManagerResult.Value);

        // if no job manager present

        var initialize = await InitializeNewJobManager(repository, uow, token);

        if (initialize.IsFailed)
            return Result.Fail(initialize.Errors);

        return initialize.Value;
    }

    private async Task<Result<JobManager>> InitializeNewJobManager(
        IJobManagerRepository repository,
        IUnitOfWork uow,
        CancellationToken token)
    {
        var newJobManager = JobManager.Create();

        if (newJobManager.IsFailed)
            return Result.Fail(newJobManager.Errors);

        var persist = await repository.Persist(newJobManager.Value, token);

        if (persist.IsFailed)
            return Result.Fail(persist.Errors);

        var commit = await uow.Commit(token);

        if (commit.IsFailed)
            return Result.Fail(commit.Errors);

        return Result.Ok(newJobManager.Value);
    }

    public override string ToString() => nameof(JobManagerService);

    #endregion
}
