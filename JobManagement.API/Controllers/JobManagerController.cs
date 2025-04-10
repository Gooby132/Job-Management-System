﻿using JobManagement.API.Contracts.Commons.Dtos;
using JobManagement.API.Contracts.JobManager.Requests;
using JobManagement.API.Contracts.JobManager.Responses;
using JobManagement.API.Helpers;
using JobManagement.Domain.Common;
using JobManagement.Domain.JobManagers;
using JobManagement.Domain.JobManagers.Jobs;
using JobManagement.Domain.JobManagers.Jobs.Abstractions;
using JobManagement.Domain.JobManagers.Jobs.Errors;
using JobManagement.Domain.JobManagers.Jobs.ValueObjects;
using JobManagement.Domain.JobManagers.Services;
using JobManagement.Domain.Users;
using JobManagement.Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobManagerController : ControllerBase
{
    private readonly ILogger<JobManagerController> _logger;
    private readonly IJobManagerRepository _jobManagerRepository;
    private readonly IUnitOfWork _uow;
    private readonly IExecutionProvider _jobProvider;
    private readonly IJobExecutionBag _executionBag;

    public JobManagerController(
        ILogger<JobManagerController> logger,
        IJobManagerRepository jobManagerRepository,
        IUnitOfWork uow,
        IExecutionProvider jobProvider,
        IJobExecutionBag executionBag)
    {
        _logger = logger;
        _jobManagerRepository = jobManagerRepository;
        _uow = uow;
        _jobProvider = jobProvider;
        _executionBag = executionBag;
    }

    [HttpGet("get-job")]
    public async Task<IActionResult> GetJob([FromQuery] GetJobRequest request, CancellationToken token = default)
    {
        var jobName = JobName.Create(request.JobName);

        if (jobName.IsFailed)
            return BadRequest(new AppendJobResponse
            {
                Errors = jobName.Errors.ToDtos()
            });

        var jobManager = await _jobManagerRepository.GetJobManager(token);

        if (jobManager.IsFailed)
        {
            _logger.LogWarning("{this} failed fetching repository. errors - {errors}",
                this, string.Join(", ", jobManager.Errors.Select(e => e.Message)));

            return BadRequest(new GetJobResponse
            {
                Errors = jobManager.Errors.ToDtos()
            });
        }

        if (jobManager.Value is null)
        {
            _logger.LogError("{this} job manager was not found",
                this);

            return Problem("repository error");
        }

        var job = jobManager.Value.GetJobByJobName(jobName.Value);

        if (job is null)
            return NotFound(new GetJobResponse
            {
                Errors = [JobsErrorFactory.JobWasNotFound().ToDto()]
            });

        return Ok(new GetJobResponse
        {
            Job = job.ToDto()
        });

    }

    [HttpGet("available-job-executions")]
    public IActionResult AvailableJobs(CancellationToken token = default) =>
        Ok(new AvailableJobsResponse
        {
            JobExecutions = _jobProvider.AvailableJobs
        });

    [HttpGet("jobs-statuses")]
    public async Task<IActionResult> JobsStatuses(CancellationToken token = default)
    {
        var jobManager = await _jobManagerRepository.GetJobManager(token);

        if (jobManager.IsFailed)
        {
            _logger.LogWarning("{this} failed fetching repository. errors - {errors}",
                this, string.Join(", ", jobManager.Errors.Select(e => e.Message)));

            return BadRequest(new AppendJobResponse
            {
                Errors = jobManager.Errors.ToDtos()
            });
        }

        if (jobManager.Value is null)
        {
            _logger.LogError("{this} job manager was not found",
                this);

            return Problem("repository error");
        }

        var executions = _executionBag.GetExecutions();
        var joinedJobAndExec = jobManager.Value.Jobs
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

        return Ok(new JobsStatusesResponse
        {
            Jobs = joinedJobAndExec.ToDtos()
        });
    }

    [HttpPost("append-job")]
    public async Task<IActionResult> AppendJob(AppendJobRequest request, CancellationToken token)
    {
        var jobName = JobName.Create(request.JobName);
        var jobExecName = JobExecutionName.Create(request.JobExecutionName);

        if (jobName.IsFailed)
            return BadRequest(new AppendJobResponse
            {
                Errors = jobName.Errors.ToDtos()
            });

        if (jobExecName.IsFailed)
            return BadRequest(new AppendJobResponse
            {
                Errors = jobExecName.Errors.ToDtos()
            });

        var job = Job.Create(
            jobName.Value,
            jobExecName.Value,
            request.PriorityValue,
            request.ExecutionTimeInUtc
        );

        if (job.IsFailed)
            return BadRequest(new AppendJobResponse
            {
                Errors = job.Errors.ToDtos()
            });

        var jobManager = await _jobManagerRepository.GetJobManager(token);

        if (jobManager.IsFailed)
        {
            _logger.LogWarning("{this} failed fetching repository. errors - {errors}",
                this, string.Join(", ", jobManager.Errors.Select(e => e.Message)));

            return BadRequest(new AppendJobResponse
            {
                Errors = jobManager.Errors.ToDtos()
            });
        }

        if (jobManager.Value is null)
        {
            _logger.LogError("{this} job manager was not found",
                this);

            return Problem("repository error");
        }

        var append = jobManager.Value.AppendJob(job.Value);

        if (append.IsFailed)
        {
            _logger.LogWarning("{this} failed starting job. errors - {errors}",
                this, string.Join(", ", append.Errors.Select(e => e.Message)));

            return UnprocessableEntity(new AppendJobResponse
            {
                Errors = append.Errors.ToDtos()
            });
        }

        var commit = await _uow.Commit();

        if (commit.IsFailed)
        {
            _logger.LogError("{this} failed committing job manager state. errors - {errors}",
                this, string.Join(", ", commit.Errors.Select(e => e.Message)));

            return Problem("commit error");
        }

        return Ok(new AppendJobResponse
        {
            Job = job.Value.ToDto(),
        });
    }

    [HttpPost("start-job")]
    public async Task<IActionResult> StartJob(StartJobRequest request, CancellationToken cancellationToken)
    {

        var jobName = JobName.Create(request.JobName);

        if (jobName.IsFailed)
        {
            _logger.LogWarning("{this} job name provided is invalid. errors - {errors}",
                this, string.Join(", ", jobName.Errors.Select(e => e.Message)));

            return BadRequest(new StartJobResponse
            {
                Errors = jobName.Errors.ToDtos()
            });
        }

        var jobManager = await _jobManagerRepository.GetJobManager(cancellationToken);

        if (jobManager.IsFailed)
        {
            _logger.LogWarning("{this} failed fetching repository. errors - {errors}",
                this, string.Join(", ", jobManager.Errors.Select(e => e.Message)));

            return BadRequest(new StartJobResponse
            {
                Errors = jobManager.Errors.ToDtos()
            });
        }

        if (jobManager.Value is null)
        {
            _logger.LogError("{this} job manager was not found",
                this);

            return Problem("repository error");
        }

        var startJob = jobManager.Value.StartJob(jobName.Value, _jobProvider, _executionBag);

        if (startJob.IsFailed)
        {
            _logger.LogWarning("{this} failed starting job. errors - {errors}",
                this, string.Join(", ", startJob.Errors.Select(e => e.Message)));

            return UnprocessableEntity(new StartJobResponse
            {
                Errors = startJob.Errors.ToDtos()
            });
        }

        var commit = await _uow.Commit();

        if (commit.IsFailed)
        {
            _logger.LogError("{this} failed committing job manager state. errors - {errors}",
                this, string.Join(", ", commit.Errors.Select(e => e.Message)));

            return Problem("commit error");
        }

        return Ok(new StartJobResponse()
        {
            Job = startJob.Value.ToDto()
        });
    }

    [HttpPost("request-stop-job")]
    public async Task<IActionResult> RequestStopJob(RequestStopJobRequest request, CancellationToken cancellationToken)
    {

        var jobName = JobName.Create(request.JobName);

        if (jobName.IsFailed)
        {
            _logger.LogWarning("{this} job name provided is invalid. errors - {errors}",
                this, string.Join(", ", jobName.Errors.Select(e => e.Message)));

            return BadRequest(new RequestStopJobResponse
            {
                Errors = jobName.Errors.ToDtos()
            });
        }

        var jobManager = await _jobManagerRepository.GetJobManager(cancellationToken);

        if (jobManager.IsFailed)
        {
            _logger.LogWarning("{this} failed fetching repository. errors - {errors}",
                this, string.Join(", ", jobManager.Errors.Select(e => e.Message)));

            return BadRequest(new RequestStopJobResponse
            {
                Errors = jobManager.Errors.ToDtos()
            });
        }

        if (jobManager.Value is null)
        {
            _logger.LogError("{this} job manager was not found",
                this);

            return Problem("repository error");
        }

        var stopJob = jobManager.Value.RequestStopJob(jobName.Value, _executionBag);

        if (stopJob.IsFailed)
        {
            _logger.LogWarning("{this} failed stopping job. errors - {errors}",
                this, string.Join(", ", stopJob.Errors.Select(e => e.Message)));

            return UnprocessableEntity(new RequestStopJobResponse
            {
                Errors = stopJob.Errors.ToDtos()
            });
        }

        var commit = await _uow.Commit();

        if (commit.IsFailed)
        {
            _logger.LogError("{this} failed committing job manager state. errors - {errors}",
                this, string.Join(", ", commit.Errors.Select(e => e.Message)));

            return Problem("commit error");
        }

        return Ok(new RequestStopJobResponse()
        {
            Job = stopJob.Value.ToDto()
        });
    }

    [HttpPost("restart-job")]
    public async Task<IActionResult> RestartJob(RestartJobRequest request, CancellationToken cancellationToken)
    {

        var jobName = JobName.Create(request.JobName);

        if (jobName.IsFailed)
        {
            _logger.LogWarning("{this} job name provided is invalid. errors - {errors}",
                this, string.Join(", ", jobName.Errors.Select(e => e.Message)));

            return BadRequest(new RestartJobResponse
            {
                Errors = jobName.Errors.ToDtos()
            });
        }

        var jobManager = await _jobManagerRepository.GetJobManager(cancellationToken);

        if (jobManager.IsFailed)
        {
            _logger.LogWarning("{this} failed fetching repository. errors - {errors}",
                this, string.Join(", ", jobManager.Errors.Select(e => e.Message)));

            return BadRequest(new RestartJobResponse
            {
                Errors = jobManager.Errors.ToDtos()
            });
        }

        if (jobManager.Value is null)
        {
            _logger.LogError("{this} job manager was not found",
                this);

            return Problem("repository error");
        }

        var restartJob = jobManager.Value.RestartJob(jobName.Value, _executionBag);

        if (restartJob.IsFailed)
        {
            _logger.LogWarning("{this} failed restarting job. errors - {errors}",
                this, string.Join(", ", restartJob.Errors.Select(e => e.Message)));

            return UnprocessableEntity(new RestartJobResponse
            {
                Errors = restartJob.Errors.ToDtos()
            });
        }

        var commit = await _uow.Commit();

        if (commit.IsFailed)
        {
            _logger.LogError("{this} failed committing job manager state. errors - {errors}",
                this, string.Join(", ", commit.Errors.Select(e => e.Message)));

            return Problem("commit error");
        }

        return Ok(new RestartJobResponse()
        {
            Job = restartJob.Value.ToDto(),
        });
    }

    [Authorize(Roles = nameof(UserRole.Operator))]
    [HttpPost("delete-job")]
    public async Task<IActionResult> DeleteJob(DeleteJobRequest request, CancellationToken cancellationToken)
    {

        var jobName = JobName.Create(request.JobName);

        if (jobName.IsFailed)
        {
            _logger.LogWarning("{this} job name provided is invalid. errors - {errors}",
                this, string.Join(", ", jobName.Errors.Select(e => e.Message)));

            return BadRequest(new DeleteJobResponse
            {
                Errors = jobName.Errors.ToDtos()
            });
        }

        var jobManager = await _jobManagerRepository.GetJobManager(cancellationToken);

        if (jobManager.IsFailed)
        {
            _logger.LogWarning("{this} failed fetching repository. errors - {errors}",
                this, string.Join(", ", jobManager.Errors.Select(e => e.Message)));

            return BadRequest(new DeleteJobResponse
            {
                Errors = jobManager.Errors.ToDtos()
            });
        }

        if (jobManager.Value is null)
        {
            _logger.LogError("{this} job manager was not found",
                this);

            return Problem("repository error");
        }

        var delete = jobManager.Value.RequestDeleteJob(jobName.Value, _executionBag);

        if (delete.IsFailed)
        {
            _logger.LogWarning("{this} failed deleting job. errors - {errors}",
                this, string.Join(", ", delete.Errors.Select(e => e.Message)));

            return UnprocessableEntity(new DeleteJobResponse
            {
                Errors = delete.Errors.ToDtos()
            });
        }

        var commit = await _uow.Commit();

        if (commit.IsFailed)
        {
            _logger.LogError("{this} failed committing job manager state. errors - {errors}",
                this, string.Join(", ", commit.Errors.Select(e => e.Message)));

            return Problem("commit error");
        }

        return Ok(new DeleteJobResponse()
        {
            Job = delete.Value.ToDto(),
        });
    }

    public override string ToString() => nameof(JobManagerController);

}
