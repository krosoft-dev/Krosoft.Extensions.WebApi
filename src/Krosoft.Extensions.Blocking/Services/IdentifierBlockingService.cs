using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.Blocking.Abstractions.Models.Enums;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Blocking.Services;

public class IdentifierBlockingService : BlockingService, IIdentifierBlockingService
{
    private readonly IIdentifierProvider _identifierProvider;

    public IdentifierBlockingService(IBlockingStorageProvider blockingStorageProvider,
                                     ILogger<IdentifierBlockingService> logger,
                                     IIdentifierProvider identifierProvider)
        : base(BlockType.Identifier, blockingStorageProvider, logger)
    {
        _identifierProvider = identifierProvider;
    }

    public async Task BlockAsync(CancellationToken cancellationToken)
    {
        var identifier = await GetIdentifierAsync(cancellationToken);
        await BlockAsync(identifier, cancellationToken);
    }

    public async Task<bool> IsBlockedAsync(CancellationToken cancellationToken)
    {
        var identifier = await GetIdentifierAsync(cancellationToken);
        var isBlocked = await IsBlockedAsync(identifier, cancellationToken);
        return isBlocked;
    }

    private async Task<string> GetIdentifierAsync(CancellationToken cancellationToken)
    {
        var identifier = await _identifierProvider.GetIdentifierAsync(cancellationToken);
        if (string.IsNullOrEmpty(identifier))
        {
            throw new KrosoftTechniqueException("Impossible d'obtenir l'identifiant !");
        }

        return identifier;
    }
}