using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.WebApi.Identity.Middlewares;
using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Identity.Services;

internal class HttpAgentIdProvider : IAgentIdProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpAgentIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<string?> GetAgentIdAsync(CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            if (headers.TryGetValue(AgentIdMiddleware.AgentIdHeaderName, out var apiKeyValues))
            {
                var apiKey = apiKeyValues.FirstOrDefault();
                return Task.FromResult(apiKey);
            }

            return Task.FromResult<string?>(null);
        }

        throw new KrosoftTechnicalException("HttpContext non défini.");
    }
}