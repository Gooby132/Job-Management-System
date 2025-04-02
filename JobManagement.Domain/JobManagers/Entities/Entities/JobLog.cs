namespace JobManagement.Domain.JobManagers.Entities.Entities;

public class JobLog
{

    public const int MaxLength = 5_000;
    public string Value { get; private set; } = string.Empty;

    public JobLog() { }

    public void Append(string log)
    {
        var time = DateTime.UtcNow.ToString("O");

        if (Value.Length > MaxLength)
            Value = Value.Substring(time.Length + log.Length, MaxLength - 1);

        Value += $"{time}: {log}";
    }

}
