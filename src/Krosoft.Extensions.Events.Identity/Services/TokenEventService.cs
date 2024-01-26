using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Events.Identity.Interfaces;
using Krosoft.Extensions.Events.Services;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Jobs.Interfaces;
using MediatR;

namespace Krosoft.Extensions.Events.Identity.Services;

public class TokenEventService : EventService, ITokenEventService
{
    private readonly IFireForgetService _fireForgetService;
    private readonly IKrosoftTokenBuilderService _positiveTokenBuilderService;

    public TokenEventService(IFireForgetService fireForgetService,
                             IKrosoftTokenBuilderService positiveTokenBuilderService) : base(fireForgetService)
    {
        _fireForgetService = fireForgetService;
        _positiveTokenBuilderService = positiveTokenBuilderService;
    }

    public void Publish(Func<KrosoftToken, INotification> func, CancellationToken cancellationToken)
    {
        _fireForgetService.FireAsync<IMediator>(async mediator =>
        {
            var positiveToken = _positiveTokenBuilderService.Build();

            await mediator.Publish(func(positiveToken), cancellationToken);
        });
    }
}