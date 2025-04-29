using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Krosoft.Extensions.Jobs.Hangfire.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseHangfire(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[]
            {
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = configuration.GetValue<string>("JobSettings:Username"),
                    Pass = configuration.GetValue<string>("JobSettings:Password")
                }
            }
        });
    }
}