namespace JobManagement.API.Contracts.JobManager.Requests;

public class AppendJobRequest
{
    public string? JobName { get; init; }
    public int PriorityValue { get; init; }
    public string? ExecutionTimeInUtc { get; init; }
    public string? JobExecutionName { get; init; }
}
