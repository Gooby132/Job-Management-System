using FluentResults;
using JobManagement.Domain.Users.Errors;

namespace JobManagement.Domain.Users.ValueObjects;

public class UserPassword
{

    public const int MaxUserPasswordLength = 100;
    public const int MinUserPasswordLength = 10;

    public required string Value { get; init; }

    private UserPassword() { }

    public static Result<UserPassword> Create(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return UserErrorFactory.UserNameIsInvalid();

        if (password.Length < MinUserPasswordLength)
            return UserErrorFactory.UserNameIsTooShort();

        if (password.Length > MaxUserPasswordLength)
            return UserErrorFactory.UserNameIsTooLong();

        return new UserPassword { Value = password };
    }

    public bool IsMatch(UserPassword other) => Value == other.Value;

}
