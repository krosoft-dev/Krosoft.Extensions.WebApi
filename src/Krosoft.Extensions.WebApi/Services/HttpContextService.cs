using Krosoft.Extensions.WebApi.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Services;

public class HttpContextService : IHttpContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetBaseUrl()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var host = _httpContextAccessor.HttpContext.Request.Host.Value;
            var scheme = _httpContextAccessor.HttpContext.Request.Scheme;
            return $"{scheme}://{host}/";
        }

        return string.Empty;
    }

    public IEnumerable<string> GetInformations()
    {
        var informations = new List<string>();

        if (_httpContextAccessor.HttpContext != null)
        {
            informations.Add($"HttpContext.Connection.RemoteIpAddress : {_httpContextAccessor.HttpContext.Connection.RemoteIpAddress}");
            informations.Add($"HttpContext.Connection.RemoteIpPort : {_httpContextAccessor.HttpContext.Connection.RemotePort}");
            informations.Add($"HttpContext.Request.Scheme : {_httpContextAccessor.HttpContext.Request.Scheme}");
            informations.Add($"HttpContext.Request.Host : {_httpContextAccessor.HttpContext.Request.Host}");

            var headers = _httpContextAccessor.HttpContext.Request.Headers
                                              .Where(h => h.Key.StartsWith("X", StringComparison.OrdinalIgnoreCase))
                                              .ToList();
            foreach (var header in headers)
            {
                informations.Add($"Request-Header {header.Key}: {header.Value}");
            }
        }

        return informations;
    }
}