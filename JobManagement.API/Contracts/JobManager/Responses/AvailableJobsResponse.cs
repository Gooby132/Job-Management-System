namespace JobManagement.API.Contracts.JobManager.Responses;

public class AvailableJobsResponse
{

    public required IEnumerable<string> JobExecutions { get; init; }

}
