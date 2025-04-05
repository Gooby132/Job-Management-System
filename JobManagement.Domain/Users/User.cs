using FluentResults;
using JobManagement.Domain.Users.ValueObjects;

namespace JobManagement.Domain.Users;

public class User
{

    public required UserName Name { get; init; }
    public required UserPassword Password { get; init; }
    public required UserRole Role { get; init; }

    private User() { }

    public static Result<User> Create(
        UserName name,
        UserPassword password,
        UserRole userRole
        )
    {
        return new User { 
            Name = name, 
            Password = password,
            Role = userRole 
        };
    }

    public bool IsPasswordsMatch(UserPassword password) => Password.IsMatch(password); 

}
