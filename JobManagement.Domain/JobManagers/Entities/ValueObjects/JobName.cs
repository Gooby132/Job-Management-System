using FluentResults;

namespace JobManagement.Domain.JobManagers.Entities.ValueObjects;

public class JobName
{

    public required string Value { get; init; }

    // only constructible by factory method
    private JobName() { }

    // validation on job name
    public static Result<JobName> Create(string name)
    {


        return new JobName { Value = name };
    }

    public override string ToString() => Value;

}
