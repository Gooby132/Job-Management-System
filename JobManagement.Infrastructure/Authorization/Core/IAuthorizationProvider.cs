using JobManagement.Domain.Users;

namespace JobManagement.Infrastructure.Authorization.Core;

public interface IAuthorizationProvider
{

    public const string Name= "Jwt";

    public string AuthorizeAdmin(User user);

}
