using System.Net.Mime;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.WebApi.HealthChecks.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Krosoft.Extensions.WebApi.HealthChecks.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseHealthChecksExt(this IApplicationBuilder builder,
                                                         IHostEnvironment env)
    {
        return builder.UseHealthChecks(Urls.Health.Check, new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            },
            ResponseWriter = async (context,
                                    report) =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;

                var response = new HealthCheckStatusModel
                {
                    Status = report.Status.ToString(),
                    Checks = report.Entries.Select(x => new HealthCheckModel
                    {
                        Key = x.Key,
                        Status = x.Value.Status.ToString(),
                        Description = x.Value.Description
                    }),
                    Duration = report.TotalDuration.ToShortString(),
                    Environnement = env.EnvironmentName
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        });
    }
}