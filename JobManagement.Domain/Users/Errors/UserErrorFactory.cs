using JobManagement.Domain.Common;

namespace JobManagement.Domain.Users.Errors;

public static class UserErrorFactory
{

    public const int GroupCode = 2;

    public static ErrorBase UserNameIsInvalid() => new(GroupCode, 1, "user name is invalid");
    public static ErrorBase UserNameIsTooShort() => new(GroupCode, 2, "user name too short");
    public static ErrorBase UserNameIsTooLong() => new(GroupCode, 3, "user name too long");

}
