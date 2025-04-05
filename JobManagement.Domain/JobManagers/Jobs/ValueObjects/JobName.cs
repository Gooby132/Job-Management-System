using FluentResults;
using JobManagement.Domain.Common;
using JobManagement.Domain.JobManagers.Jobs.Errors;

namespace JobManagement.Domain.JobManagers.Jobs.ValueObjects;

public class JobName : IValueObject
{

    public const int MaxJobNameLength = 50;
    public const int MinJobNameLength = 2;

    public required string Value { get; init; }

    // only constructible by factory method
    private JobName() { }

    // validation on job name
    public static Result<JobName> Create(string? name)
    {

        if (string.IsNullOrEmpty(name))
            return JobsErrorFactory.JobNameIsInvalid();

        if (name.Length > MaxJobNameLength)
            return JobsErrorFactory.JobNameIsTooLong();

        if (name.Length < MinJobNameLength)
            return JobsErrorFactory.JobNameIsTooShort();

        return new JobName { Value = name };
    }

    public override string ToString() => Value;

    public static bool operator ==(JobName left, JobName right) => left.Equals(right);
    public static bool operator !=(JobName left, JobName right) => !left.Equals(right);

    public override bool Equals(object? obj) => obj is JobName && ((JobName)obj).Value == Value;

    public override int GetHashCode() => Value.GetHashCode();

}
