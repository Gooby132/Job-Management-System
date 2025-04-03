using FluentResults;

namespace JobManagement.Domain.JobManagers;

public interface IJobManagerRepository
{

    public Task<Result<JobManager?>> GetJobManager(CancellationToken token = default);

    public Task<Result> Persist(JobManager jobManager, CancellationToken token = default);

    public Result Update(JobManager jobManager, CancellationToken token = default);

}
