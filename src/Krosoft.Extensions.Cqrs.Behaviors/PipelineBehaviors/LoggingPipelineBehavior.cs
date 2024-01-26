using System.Diagnostics;
using Krosoft.Extensions.Core.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Cqrs.Behaviors.PipelineBehaviors;

public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling <{typeof(TRequest).Name},{typeof(TResponse).Name}>");
        var sw = Stopwatch.StartNew();
        var response = await next();
        _logger.LogInformation($"Handled <{typeof(TRequest).Name},{typeof(TResponse).Name}> in {sw.Elapsed.ToShortString()}");

        return response;
    }
}