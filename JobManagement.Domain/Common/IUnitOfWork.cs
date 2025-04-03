using FluentResults;

namespace JobManagement.Domain.Common;

public interface IUnitOfWork
{

    public Task<Result> Commit(CancellationToken cancellationToken = default);
    public Task<Result> Rollback(CancellationToken cancellationToken = default);

}
