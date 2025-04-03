using FluentResults;
using JobManagement.Domain.JobManagers.Entities.Errors;

namespace JobManagement.Domain.JobManagers.Entities.ValueObjects;

public class JobExecutionName
{
    public const int MaxNameLength = 50;
    public const int MinNameLength = 2;

    public required string Value { get; init; }

    private JobExecutionName() { }

    public static Result<JobExecutionName> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return JobsErrorFactory.JobExecutionNameIsInvalid();

        if (name.Length < MinNameLength)
            return JobsErrorFactory.JobExecutionNameIsTooShort();

        if (name.Length > MaxNameLength)
            return JobsErrorFactory.JobExecutionNameIsTooLong();

        return new JobExecutionName { Value = name };
    }

    public override string ToString() => Value;

}
