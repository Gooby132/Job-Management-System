using FluentResults;
using JobManagement.Domain.JobManagers;
using JobManagement.Domain.JobManagers.Entities.Errors;
using JobManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobManagement.Persistence.JobManagers;

internal class JobManagerRepository : IJobManagerRepository
{
    private readonly ApplicationContext _context;

    public JobManagerRepository(ILogger<JobManagerRepository> logger, ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Result<JobManager?>> GetJobManager(CancellationToken token = default)
    {
        try
        {
            return await _context.JobManagers.FirstOrDefaultAsync(token);
        }
        catch (Exception e)
        {
            return Result.Fail(JobsErrorFactory.CouldNotFetchJobManagerFromRepository().CausedBy(e));
        }
    }

    public async Task<Result> Persist(JobManager jobManager, CancellationToken token = default)
    {
        try
        {
            await _context.JobManagers.AddAsync(jobManager, token);
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(JobsErrorFactory.FailedJobManagerToRepository().CausedBy(e));
        }
    }

    public Result Update(JobManager jobManager, CancellationToken token = default)
    {
        try
        {
            _context.JobManagers.Update(jobManager);
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(JobsErrorFactory.FailedUpdatingJobManager().CausedBy(e));
        }
    }
}
