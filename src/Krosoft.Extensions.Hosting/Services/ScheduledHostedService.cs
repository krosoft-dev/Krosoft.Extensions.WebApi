using Krosoft.Extensions.Hosting.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Hosting.Services;

public abstract class ScheduledHostedService : IHostedService, IDisposable
{
    private const string ServiceName = nameof(ScheduledHostedService);
    private readonly TimeSpan _interval;
    protected readonly ILogger<ScheduledHostedService> Logger;
    private CancellationToken _cancellationToken;
    private bool _disposed;
    private Timer? _timer;

    protected ScheduledHostedService(ILogger<ScheduledHostedService> logger, ScheduleConfig config)
    {
        Logger = logger;
        _interval = config.Interval;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation($"{ServiceName} is starting.");
        _cancellationToken = cancellationToken;
        _timer = new Timer(DoWorkWrapper, null, TimeSpan.Zero, _interval);

        return Task.CompletedTask;
    }

    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        Logger.LogDebug($"{ServiceName} is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _timer?.Dispose();
        }

        _disposed = true;
    }

    protected abstract Task DoWork(CancellationToken cancellationToken);

    private void DoWorkWrapper(object? state)
    {
        Task.Run(async () =>
        {
            try
            {
                if (!_cancellationToken.IsCancellationRequested)
                {
                    await DoWork(_cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while executing the scheduled task.");
            }
        }, _cancellationToken);
    }
}