using JobManagement.API.Contracts.Commons.Dtos;
using JobManagement.API.Contracts.JobManager.Dtos;

namespace JobManagement.API.Contracts.JobManager.Responses;

public class DeleteJobResponse
{

    public JobDto? Job { get; init; }

    public IEnumerable<ErrorDto>? Errors { get; set; }

}
