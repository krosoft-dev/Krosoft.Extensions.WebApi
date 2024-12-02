using Krosoft.Extensions.Samples.Library.Models.Queries;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Handlers.Queries;

public class HelloDotNet9QueryHandler : IRequestHandler<HelloDotNet9Query, string>
{
    private readonly ILogger<HelloDotNet9QueryHandler> _logger;

    public HelloDotNet9QueryHandler(ILogger<HelloDotNet9QueryHandler> logger)
    {
        _logger = logger;
    }

    public Task<string> Handle(HelloDotNet9Query request,
                               CancellationToken cancellationToken)
    {
        _logger.LogInformation("Hello DotNet8...");

        return Task.FromResult("Hello DotNet8");
    }
}