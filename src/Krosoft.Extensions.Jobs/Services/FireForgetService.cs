using Krosoft.Extensions.Jobs.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Jobs.Services
{
    public class FireForgetService : IFireForgetService
    {
        private readonly ILogger<FireForgetService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public FireForgetService(ILogger<FireForgetService> logger,
                                 IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public void Fire<T>(Action<T> action) where T : notnull
        {
            _logger.LogInformation("Fired a new action.");
            Task.Run(() =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dependency = scope.ServiceProvider.GetRequiredService<T>();
                    try
                    {
                        action(dependency);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);
                    }
                }
            });
        }

        public void FireAsync<T>(Func<T, Task> func) where T : notnull
        {
            _logger.LogInformation("Fired a new async action.");
            Task.Run(async () =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dependency = scope.ServiceProvider.GetRequiredService<T>();
                    try
                    {
                        await func(dependency);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);
                    }
                }
            });
        }
    }
}