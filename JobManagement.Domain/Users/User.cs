using FluentResults;
using JobManagement.Domain.Common;
using JobManagement.Domain.Users.ValueObjects;

namespace JobManagement.Domain.Users;

/// <summary>
/// Represents a user
/// </summary>
public class User : IAggregateRoot
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
