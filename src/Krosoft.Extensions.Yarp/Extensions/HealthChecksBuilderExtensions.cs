using Krosoft.Extensions.Yarp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Yarp.Extensions;

public static class HealthChecksBuilderExtensions
{
    public static IHealthChecksBuilder AddChecksCustomReverseProxy(this IHealthChecksBuilder healthChecksBuilder,
                                                                   IConfiguration configuration,
                                                                   params string[] servicesIgnored)
    {
        var servicesName = configuration.GetSection($"{nameof(CustomReverseProxySettings)}:{nameof(CustomReverseProxySettings.Services)}").GetChildren().Select(c => c.Key);
        foreach (var serviceName in servicesName)
        {
            if (!servicesIgnored.Contains(serviceName))
            {
                healthChecksBuilder.AddUrlGroup(new Uri($"{configuration[$"{nameof(CustomReverseProxySettings)}:{nameof(CustomReverseProxySettings.Services)}:{serviceName}:{nameof(ReverseProxyService.Destination)}"]}/health/readiness"), serviceName);
            }
        }

        return healthChecksBuilder;
    }
}