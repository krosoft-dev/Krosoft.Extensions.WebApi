using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.Blocking.Abstractions.Models.Enums;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Blocking.Services;

public class AccessTokenBlockingService : BlockingService, IAccessTokenBlockingService
{
    private readonly IAccessTokenProvider _accessTokenProvider;

    public AccessTokenBlockingService(IBlockingStorageProvider blockingStorageProvider,
                                      ILogger<AccessTokenBlockingService> logger,
                                      IAccessTokenProvider accessTokenProvider)
        : base(BlockType.AccessToken, blockingStorageProvider, logger)
    {
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task<bool> IsBlockedAsync(CancellationToken cancellationToken)
    {
        var accessToken = await GetAccessTokenAsync(cancellationToken);
        var isBlocked = await IsBlockedAsync(accessToken, cancellationToken);

        return isBlocked;
    }

    public async Task BlockAsync(CancellationToken cancellationToken)
    {
        var accessToken = await GetAccessTokenAsync(cancellationToken);
        await BlockAsync(accessToken, cancellationToken);
    }

    private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        var accessToken = await _accessTokenProvider.GetAccessTokenAsync(cancellationToken);
        if (string.IsNullOrEmpty(accessToken))
        {
            throw new KrosoftTechniqueException("Impossible d'obtenir le token d'accès !");
        }

        return accessToken;
    }
}