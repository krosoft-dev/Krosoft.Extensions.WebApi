using Krosoft.Extensions.Samples.DotNet6.Api.Models.Queries;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Handlers.Queries;

public class HelloWorldQueryHandler : IRequestHandler<HelloWorlQuery, string>
{
    private readonly ILogger<HelloWorldQueryHandler> _logger;

    public HelloWorldQueryHandler(ILogger<HelloWorldQueryHandler> logger)
    {
        _logger = logger;
    }

    public Task<string> Handle(HelloWorlQuery request,
                               CancellationToken cancellationToken)
    {
        _logger.LogInformation("Hello World...");

        return Task.FromResult("Hello World");
    }
}