using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.DelegatingHandlers;

public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //On affecte le token que s'il n'y en pas déjà un.
        if (request.Headers.Authorization == null && _httpContextAccessor.HttpContext != null)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                if (token != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}