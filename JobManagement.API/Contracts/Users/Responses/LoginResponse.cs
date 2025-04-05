using JobManagement.API.Contracts.Commons.Dtos;

namespace JobManagement.API.Contracts.Users.Responses;

public class LoginResponse
{

    public IEnumerable<ErrorDto>? Errors { get; init; }
    public string? Token { get; set; }

}
