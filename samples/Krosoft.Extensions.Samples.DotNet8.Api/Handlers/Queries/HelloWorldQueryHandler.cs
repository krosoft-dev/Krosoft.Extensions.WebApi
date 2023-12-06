using Krosoft.Extensions.Samples.DotNet8.Api.Models.Queries;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Handlers.Queries;

public class HelloDotNet8QueryHandler : IRequestHandler<HelloDotNet8Query, string>
{
    private readonly ILogger<HelloDotNet8QueryHandler> _logger;

    public HelloDotNet8QueryHandler(ILogger<HelloDotNet8QueryHandler> logger)
    {
        _logger = logger;
    }

    public Task<string> Handle(HelloDotNet8Query request,
                               CancellationToken cancellationToken)
    {
        _logger.LogInformation("Hello DotNet8...");

        return Task.FromResult("Hello DotNet8");
    }
}