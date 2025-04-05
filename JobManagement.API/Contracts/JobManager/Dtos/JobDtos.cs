namespace JobManagement.API.Contracts.JobManager.Dtos;

public class JobDto
{

    public string? Name { get; init; }
    public string? ExecutionName { get; init; }
    public int PriorityValue { get; init; }
    public int StatusValue { get; init; }
    public int Progress { get; init; }
    public string? CreatedInUtc { get; init; }
    public string? ExecutionTimeInUtc { get; init; }
    public string? StartTimeInUtc { get; init; }
    public string? EndTimeInUtc { get; init; }
    public string? Log { get; init; }

}
