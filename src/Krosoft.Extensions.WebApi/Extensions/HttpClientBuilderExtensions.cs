using Krosoft.Extensions.WebApi.DelegatingHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddAuthorizationHandler(this IHttpClientBuilder httpClientBuilder,
                                                             IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
        httpClientBuilder.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

        return httpClientBuilder;
    }

    public static IHttpClientBuilder AddHandlers(this IHttpClientBuilder httpClientBuilder,
                                                 IServiceCollection services,
                                                 bool useAuthorization)
    {
        if (useAuthorization)
        {
            httpClientBuilder.AddAuthorizationHandler(services);
        }

        return httpClientBuilder;
    }
}