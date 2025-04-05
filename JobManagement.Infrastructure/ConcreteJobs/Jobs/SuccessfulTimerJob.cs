using FluentResults;
using JobManagement.Domain.JobManagers.Jobs.Abstractions;
using JobManagement.Domain.JobManagers.Jobs.Errors;
using JobManagement.Domain.JobManagers.Jobs.ValueObjects;

namespace JobManagement.Infrastructure.ConcreteJobs.Jobs;

public class SuccessfulTimerJob : IJobExecution
{
    #region Fields

    private CancellationTokenSource _cancellationTokenSource = new();
    private Task? _task;
    private int _progress = 0;
    private bool _stop = false;

    #endregion

    #region Properties

    public static JobExecutionName Name = JobExecutionName.Create(nameof(SuccessfulTimerJob)).Value;

    public int Progress => _progress;
    public bool IsFailed { get; private set; }
    public bool IsCanceled { get; private set; }
    public bool IsCompleted { get => _progress == 100 || IsFailed || IsCanceled; }

    #endregion

    #region Methods

    public void Run() =>
        _task = Inner(_cancellationTokenSource.Token);

    public async Task Inner(CancellationToken token)
    {
        try
        {
            while (_progress < 100)
            {
                if (_stop)
                    return;

                await Task.Delay(1000, token); // Simulate work being done

                if (token.IsCancellationRequested)
                    return;

                _progress++;
            }

            return;
        }
        catch (TaskCanceledException)
        {
            return;
        }
    }

    public void ForceStop()
    {
        IsCanceled = true;
        _cancellationTokenSource.Cancel();
    }

    public void Stop()
    {
        IsCanceled = true;
        _stop = true;
    }

    #endregion

}
