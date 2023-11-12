using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;

namespace Krosoft.Extensions.Identity.Services;

public class HttpContextService : IHttpContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        //On affecte le token que s'il n'y en pas déjà un.
        if (_httpContextAccessor.HttpContext != null)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                if (token != null)
                {
                    return token;
                }
            }

            return string.Empty;
        }

        throw new KrosoftTechniqueException("HttpContext non défini.");
    }
}