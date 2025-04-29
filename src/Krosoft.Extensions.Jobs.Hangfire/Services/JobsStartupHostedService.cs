using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Jobs.Hangfire.Services;

public class JobsStartupHostedService : IHostedService
{
    private readonly ILogger<JobManager> _logger;
    private readonly IServiceProvider _serviceProvider;

    public JobsStartupHostedService(IServiceProvider serviceProvider, ILogger<JobManager> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var jobManager = scope.ServiceProvider.GetRequiredService<IJobManager>();
                await jobManager.AddOrUpdateRecurringJobsAsync(cancellationToken);
            }
        }
        catch (KrosoftException ex)
        {
            _logger.LogWarning($"Un errerur est survenue lors du démarrage des flux : {ex.Message}", ex);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}