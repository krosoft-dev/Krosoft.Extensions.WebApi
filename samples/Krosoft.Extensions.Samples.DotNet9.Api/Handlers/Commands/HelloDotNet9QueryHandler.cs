using Krosoft.Extensions.Samples.Library.Models.Commands;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Handlers.Commands;

public class HelloDotNet9CommandHandler : IRequestHandler<HelloDotNet9Command, string>
{
    private readonly ILogger<HelloDotNet9CommandHandler> _logger;

    public HelloDotNet9CommandHandler(ILogger<HelloDotNet9CommandHandler> logger)
    {
        _logger = logger;
    }

    public Task<string> Handle(HelloDotNet9Command request,
                               CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Hello {request.Name}...");

        return Task.FromResult($"Hello {request.Name} !");
    }
}