using Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Commands;
using Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Handlers.Events;

public class GlobalRefreshCacheEventHandler : INotificationHandler<GlobalRefreshCacheEvent>
{
    private readonly ILogger<GlobalRefreshCacheEventHandler> _logger;
    private readonly IMediator _mediator;

    public GlobalRefreshCacheEventHandler(ILogger<GlobalRefreshCacheEventHandler> logger,
                                          IMediator mediator)
    {
        _logger = logger;

        _mediator = mediator;
    }

    public async Task Handle(GlobalRefreshCacheEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Mise à jour du cache global...");
        await _mediator.Send(new GlobalCacheRefreshCommand(), cancellationToken);
    }
}