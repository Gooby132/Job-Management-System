using JobManagement.API.Contracts.Commons.Dtos;
using JobManagement.API.Contracts.JobManager.Dtos;

namespace JobManagement.API.Contracts.JobManager.Responses;

public class GetJobResponse
{

    public IEnumerable<ErrorDto>? Errors { get; init; }

    public JobDto? Job { get; init; }

}
