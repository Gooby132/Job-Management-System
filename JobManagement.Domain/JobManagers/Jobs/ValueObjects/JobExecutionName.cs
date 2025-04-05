using FluentResults;
using JobManagement.Domain.Common;
using JobManagement.Domain.JobManagers.Jobs.Errors;

namespace JobManagement.Domain.JobManagers.Jobs.ValueObjects;

public class JobExecutionName : IValueObject
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
