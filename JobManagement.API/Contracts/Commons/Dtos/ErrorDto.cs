namespace JobManagement.API.Contracts.Commons.Dtos;

public class ErrorDto
{

    public int GroupCode { get; init; }
    public int ErrorCode { get; init; }
    public required string Message { get; init; }

}
