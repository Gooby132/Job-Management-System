using FluentResults;
using JobManagement.Domain.Users.ValueObjects;

namespace JobManagement.Domain.Users;

public interface IUserRepository
{

    public Task<Result<User>> GetUserByName(UserName name, CancellationToken token = default);
    public Task<Result> Persist(User user, CancellationToken token = default);

}
