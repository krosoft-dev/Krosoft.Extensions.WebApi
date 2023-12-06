using HealthChecks.UI.Client;
using Krosoft.Extensions.WebApi.HealthChecks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;

namespace Krosoft.Extensions.WebApi.HealthChecks.Extensions;

public static class ServiceCollectionExtensions
{
    public static void MapHealthChecksExt(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks(Urls.Health.Readiness, new HealthCheckOptions
                 {
                     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                 })
                 .WithMetadata(new AllowAnonymousAttribute());

        endpoints.MapHealthChecks(Urls.Health.Liveness, new HealthCheckOptions
                 {
                     Predicate = r => r.Name.Contains("self"),
                     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                 })
                 .WithMetadata(new AllowAnonymousAttribute());
    }
}