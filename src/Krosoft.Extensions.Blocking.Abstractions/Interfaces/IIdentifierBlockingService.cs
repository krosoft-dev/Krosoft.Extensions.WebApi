namespace Krosoft.Extensions.Blocking.Abstractions.Interfaces;

public interface IIdentifierBlockingService : IBlockingService
{
    Task BlockAsync(CancellationToken cancellationToken);
    Task<bool> IsBlockedAsync(CancellationToken cancellationToken);
}