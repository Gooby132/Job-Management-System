using Ardalis.SmartEnum;

namespace JobManagement.Domain.JobManagers.Jobs.ValueObjects;

public class JobPriority : SmartEnum<JobPriority>
{

    public static readonly JobPriority High = new JobPriority(nameof(High), 1);
    public static readonly JobPriority Regular = new JobPriority(nameof(Regular), 2);

    private JobPriority(string name, int value) :
        base(name, value) { }
}
