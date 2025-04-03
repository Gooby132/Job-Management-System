using JobManagement.API.Contracts.Commons.Dtos;
using JobManagement.API.Contracts.JobManager.Dtos;

namespace JobManagement.API.Contracts.JobManager.Responses;

public class AppendJobResponse
{

    public IEnumerable<ErrorDto>? Errors { get; init; }

    public JobDto? Job { get; set; }

}
