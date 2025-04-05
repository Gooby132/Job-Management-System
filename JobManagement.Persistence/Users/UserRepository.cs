using FluentResults;
using JobManagement.Domain.Users;
using JobManagement.Domain.Users.Errors;
using JobManagement.Domain.Users.ValueObjects;
using JobManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace JobManagement.Persistence.Users;

internal class UserRepository : IUserRepository
{

    #region Fields
    
    private readonly ILogger<UserRepository> _logger;
    private readonly ApplicationContext _context;

    #endregion

    public UserRepository(ILogger<UserRepository> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<User?>> GetUserByName(UserName name, CancellationToken token = default)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == name, token);
        }
        catch (Exception e)
        {
            return UserErrorFactory
                .RepositoryError()
                .CausedBy(e);
        }
    }

    public async Task<Result> Persist(User user, CancellationToken token = default)
    {
        try
        {
            await _context.Users.AddAsync(user, token);
            return Result.Ok();
        }
        catch (Exception e)
        {
            return UserErrorFactory
                .RepositoryError()
                .CausedBy(e);
        }
    }
}
