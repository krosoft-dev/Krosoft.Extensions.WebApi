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
        //On affecte le token que s'il n'y en pas déjà un.
        if (_httpContextAccessor.HttpContext != null)
        {
            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            if (headers.ContainsKey(ApiKeyMiddleware.ApiKeyHeaderName))
            {
                string? apiKey = headers[ApiKeyMiddleware.ApiKeyHeaderName];
                return Task.FromResult<string?>(apiKey);
            }
        }

        throw new KrosoftTechnicalException("HttpContext non défini.");
    }
}