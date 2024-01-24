namespace Krosoft.Extensions.Blocking.Abstractions.Interfaces;

public interface IAccessTokenBlockingService : IBlockingService
{
    Task BlockAsync(CancellationToken cancellationToken);
    Task<bool> IsBlockedAsync(CancellationToken cancellationToken);
}