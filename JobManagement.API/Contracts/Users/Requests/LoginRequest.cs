namespace JobManagement.API.Contracts.Users.Requests;

public class LoginRequest
{

    public string? UserName { get; init; }
    public string? Password { get; init; }

}
