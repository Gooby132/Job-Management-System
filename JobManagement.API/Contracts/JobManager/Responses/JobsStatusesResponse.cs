using JobManagement.API.Contracts.Commons.Dtos;
using JobManagement.API.Contracts.JobManager.Dtos;

namespace JobManagement.API.Contracts.JobManager.Responses;

public class JobsStatusesResponse
{

    public IEnumerable<JobDto>? Jobs { get; init; }
    public IEnumerable<ErrorDto>? Errors { get; init; }

}
