using Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Commands;
using Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Handlers.Events;

public class KrosoftTokenRefreshCacheEventHandler : INotificationHandler<KrosoftTokenRefreshCacheEvent>
{
    private readonly ILogger<KrosoftTokenRefreshCacheEventHandler> _logger;
    private readonly IMediator _mediator;

    public KrosoftTokenRefreshCacheEventHandler(ILogger<KrosoftTokenRefreshCacheEventHandler> logger,
                                                IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(KrosoftTokenRefreshCacheEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Mise à jour du cache pour le tenant {notification.KrosoftToken.TenantId}...");

        var command = new TenantCacheRefreshCommand(false)
        {
            TenantId = notification.KrosoftToken.TenantId
        };

        await _mediator.Send(command, cancellationToken);
    }
}