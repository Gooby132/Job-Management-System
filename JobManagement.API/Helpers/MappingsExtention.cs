using FluentResults;
using JobManagement.API.Contracts.Commons.Dtos;
using JobManagement.API.Contracts.JobManager.Dtos;
using JobManagement.Domain.Common;
using JobManagement.Domain.JobManagers.Entities;
using JobManagement.Domain.JobManagers.Entities.Abstractions;

namespace JobManagement.API.Helpers;

public static class ErrorExtension
{

    public static IEnumerable<ErrorDto> ToDtos(this IEnumerable<IError> errors) =>
        errors
        .Where(e => e is ErrorBase)
        .Select(e => new ErrorDto
        {
            GroupCode = (e as ErrorBase)!.GroupCode,
            ErrorCode = (e as ErrorBase)!.ErrorCode,
            Message = e.Message
        });

    public static JobDto ToDto(this Job job, IJobExecution? execution = null) => new JobDto
    {
        Name = job.Name.Value,
        PriorityValue = job.Priority.Value,
        StatusValue = job.Status.Value,
        CreatedInUtc = job.CreatedInUtc.ToString("O"),
        EndTimeInUtc = job.EndTimeInUtc?.ToString("O"),
        ExecutionTimeInUtc = job.ExecutionTimeInUtc.ToString("O"),
        StartTimeInUtc = job.StartTimeInUtc?.ToString("O"),
        Progress = execution?.Progress ?? 0,
        Log = job.Log.Value,
    };

    public static IEnumerable<JobDto> ToDtos(this IEnumerable<Job> jobs) =>
        jobs.Select(j => j.ToDto());

    public static IEnumerable<JobDto> ToDtos(this IEnumerable<KeyValuePair<Job, IJobExecution>> jobs) =>
    jobs.Select(j => j.Key.ToDto(j.Value));
}
