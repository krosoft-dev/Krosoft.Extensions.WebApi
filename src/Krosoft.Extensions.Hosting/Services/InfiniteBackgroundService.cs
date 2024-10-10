using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Hosting.Services;

public abstract class InfiniteBackgroundService : BackgroundService
{
    private const string ServiceName = nameof(InfiniteBackgroundService);
    protected readonly ILogger<InfiniteBackgroundService> Logger;

    protected InfiniteBackgroundService(ILogger<InfiniteBackgroundService> logger)
    {
        Logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation($"{ServiceName} is starting.");
        stoppingToken.Register(() => Logger.LogInformation($"{ServiceName} background task is stopping."));
        await RunAsync(stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
        Logger.LogDebug($"{ServiceName} is stopping.");
    }

    protected abstract Task RunAsync(CancellationToken stoppingToken);
}