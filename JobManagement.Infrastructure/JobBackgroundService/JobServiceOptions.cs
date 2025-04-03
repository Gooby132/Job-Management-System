using System.ComponentModel.DataAnnotations;

namespace JobManagement.Infrastructure.JobBackgroundService;

public class JobServiceOptions
{
    public const string Key = "JobServiceOptions";

    [Required]
    [Range(1, 6_000)]
    public int ServiceInvocationIntervalInSeconds { get; init; }

}
