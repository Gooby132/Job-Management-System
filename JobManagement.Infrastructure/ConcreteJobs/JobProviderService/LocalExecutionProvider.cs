using JobManagement.Domain.JobManagers.Jobs.Abstractions;
using JobManagement.Domain.JobManagers.Jobs.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace JobManagement.Infrastructure.ConcreteJobs.JobProviderService;

public class LocalExecutionProvider : IExecutionProvider
{

    private readonly ILogger<LocalExecutionProvider> _logger;
    private IDictionary<string, Type> _availableExecutions = new ConcurrentDictionary<string, Type>();

    public IEnumerable<string> AvailableJobs => _availableExecutions.Keys;

    public LocalExecutionProvider(ILogger<LocalExecutionProvider> logger)
    {
        _logger = logger;
    }

    public void AddAvailableJobs<T>(JobExecutionName name) where T : class, IJobExecution, new()
    {
        if (_availableExecutions.ContainsKey(name.Value))
            return;

        _availableExecutions.Add(name.Value, typeof(T));

        _logger.LogDebug("{this} job execution was registered with the name - '{name}'",
            this, name);
    }

    public IJobExecution? CreateInstance(JobExecutionName name)
    {
        if (!_availableExecutions.TryGetValue(name.Value, out var type))
            return null;

        _logger.LogDebug("{this} job execution with the name - '{name}' was instantiated",
            this, name);

        return (IJobExecution)Activator.CreateInstance(type)!;
    }

    public override string ToString() => nameof(LocalExecutionProvider);

}
