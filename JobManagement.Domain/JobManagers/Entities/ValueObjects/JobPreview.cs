using JobManagement.Domain.JobManagers.Entities.Entities;

namespace JobManagement.Domain.JobManagers.Entities.ValueObjects;

public class JobPreview
{
    public required JobName Name { get; init; }
    public required JobPriority Priority { get; init; }
    public required JobStatus Status { get; init; }
    public required DateTime CreatedInUtc { get; init; }
    public required DateTime ExecutionTimeInUtc { get; init; }
    public DateTime? StartTimeInUtc { get; init; }
    public DateTime? EndTimeInUtc { get; init; }
    public required JobLog Log { get; init; } // needs to be persisted on db (should be a file on the server and not in db)
    public int Progress { get; init; }
}
