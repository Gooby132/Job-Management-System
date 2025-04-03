using FluentResults;
using JobManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace JobManagement.Persistence.Context;

internal class ApplicationUnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _context;

    public ApplicationUnitOfWork(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Result> Commit(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(new Error("context error").CausedBy(e));
        }
    }

    public async Task<Result> Rollback(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.DisposeAsync();

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(new Error("context error").CausedBy(e));
        }
    }
}
