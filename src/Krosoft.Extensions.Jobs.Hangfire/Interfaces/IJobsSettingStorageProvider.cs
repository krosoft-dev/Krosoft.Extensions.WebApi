using Krosoft.Extensions.Jobs.Hangfire.Models;

namespace Krosoft.Extensions.Jobs.Hangfire.Interfaces;

public interface IJobsSettingStorageProvider
{
    Task<IEnumerable<IJobAutomatiqueSetting>> GetAsync(CancellationToken cancellationToken);
}