using FluentResults;
using JobManagement.Domain.JobManagers.Entities.Abstractions;
using JobManagement.Domain.JobManagers.Entities.Errors;
using JobManagement.Domain.JobManagers.Entities.ValueObjects;

namespace JobManagement.Infrastructure.ConcreteJobs.Jobs;

public class SuccessfulTimerJob : IJobExecution
{
    #region Fields
    
    private CancellationTokenSource _cancellationTokenSource = new();
    private Task? _task;
    private int _progress = 0;
    private bool _force = false;

    #endregion

    #region Properties
    
    public static JobExecutionName Name = JobExecutionName.Create(nameof(SuccessfulTimerJob)).Value;
    
    public int Progress => _progress;
    public bool IsStopped { get; set; }

    #endregion

    #region Methods

    public void Run() =>
        _task = Inner(_cancellationTokenSource.Token);

    public async Task<Result> Inner(CancellationToken token)
    {
        try
        {
            while (_progress < 100)
            {
                if (_force)
                    throw new TaskCanceledException();

                await Task.Delay(1000, token); // Simulate work being done

                if (token.IsCancellationRequested)
                    return Result.Fail(JobsErrorFactory.JobCanceled());

                _progress++;
            }

            return Result.Ok();
        }
        catch (TaskCanceledException)
        {
            return Result.Fail(JobsErrorFactory.JobCanceled());
        }
    }

    public void ForceStop() => _force = true;

    public void Stop() => _cancellationTokenSource.Cancel();

    #endregion

}
