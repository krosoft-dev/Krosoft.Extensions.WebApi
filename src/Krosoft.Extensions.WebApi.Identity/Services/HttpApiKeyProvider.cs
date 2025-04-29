using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.WebApi.Identity.Middlewares;
using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Identity.Services;

internal class HttpApiKeyProvider : IApiKeyProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpApiKeyProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<string?> GetApiKeyAsync(CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            if (headers.TryGetValue(ApiKeyMiddleware.ApiKeyHeaderName, out var apiKeyValues))
            {
                var apiKey = apiKeyValues.FirstOrDefault();
                return Task.FromResult(apiKey);
            }

            return Task.FromResult<string?>(null);
        }

        throw new KrosoftTechnicalException("HttpContext non défini.");
    }
}

 