using Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Handlers.Commands;

internal class AuthCacheRefreshCommandHandler : IRequestHandler<AuthCacheRefreshCommand, Unit>
{
    private readonly ILogger<AuthCacheRefreshCommandHandler> _logger;
    private readonly IMediator _mediator;

    public AuthCacheRefreshCommandHandler(ILogger<AuthCacheRefreshCommandHandler> logger,
                                          IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(AuthCacheRefreshCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Refresh du cache...");

        var command = new TenantCacheRefreshCommand(false)
        {
            TenantId = request.TenantId
        };

        await _mediator.Send(command, cancellationToken);

        await _mediator.Send(new GlobalCacheRefreshCommand(), cancellationToken);

        return Unit.Value;
    }
}