using MediatR;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Hello.Get;

public class HelloDotNet10QueryHandler : IRequestHandler<HelloQuery, string>
{
    private readonly ILogger<HelloDotNet10QueryHandler> _logger;

    public HelloDotNet10QueryHandler(ILogger<HelloDotNet10QueryHandler> logger)
    {
        _logger = logger;
    }

    public Task<string> Handle(HelloQuery request,
                               CancellationToken cancellationToken)
    {
        _logger.LogInformation("Hello DotNet10...");

        return Task.FromResult("Hello DotNet10");
    }
}