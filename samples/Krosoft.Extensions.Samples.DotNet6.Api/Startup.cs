//using System.Reflection;
//using Microsoft.Extensions.Diagnostics.HealthChecks;
//using Krosoft.Extensions.Application.Cache.Distributed.Redis.Extensions;
//using Krosoft.Extensions.Application.Extensions;
//using Krosoft.Extensions.Application.Interfaces;
//using Krosoft.Extensions.AspNetCore.Extensions;
//using Krosoft.Extensions.Cache.Distributed.Redis.Extensions;
//using Krosoft.Extensions.Cache.Distributed.Redis.HealthChecks.Extensions;
//using Krosoft.Extensions.Data.EntityFramework.PostgreSql.Extensions;
//using Krosoft.Extensions.Identity.Cache.Distributed.Extensions;
//using Krosoft.Extensions.Identity.Extensions;
//using Krosoft.Extensions.Infrastructure.Extensions;
//using Krosoft.Extensions.Mail.Mailjet.Extensions;
//using Krosoft.Extensions.Messaging.Redis.Extensions;
//using Krosoft.Extensions.Reporting.Csv.Interfaces;
//using Krosoft.Extensions.Reporting.Csv.Services;
//using Krosoft.Extensions.Samples.DotNet6.Api.Data;
//using Krosoft.Extensions.Samples.DotNet6.Api.Interfaces;
//using Krosoft.Extensions.Samples.DotNet6.Api.Services;
//using Krosoft.Extensions.Samples.DotNet6.Api.Strategies;
//using Krosoft.Extensions.WebApi.Extensions;

using System.Reflection;
using Krosoft.Extensions.WebApi.Extensions;

namespace Krosoft.Extensions.Samples.DotNet6.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app,
                          IWebHostEnvironment env)
    {
        app.UseWebApi(env, _configuration);
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddWebApi(Assembly.GetExecutingAssembly(), _configuration);

        //
        //services.AddApplication(Assembly.GetExecutingAssembly());
        //services.AddInfrastructure(_configuration);
        //services.AddJwtAuthentication(_configuration).AddBlocking(_configuration);

        //services.AddHealthChecks()
        //        .AddCheck("test", () => HealthCheckResult.Healthy())
        //        .AddRedisCheck()
        //        .AddDbContextCheck<KrosoftExtensionTenantContext>("KrosoftExtensionTenantContext");

        ////Data.
        //services.AddRepositories();
        //services.AddDbContextPostgreSql<KrosoftExtensionTenantContext>(_configuration);

        ////Cache. 
        //services.AddDistributedCacheExt();
        //services.AddCacheHandlers();
        //services.AddCacheRefreshHostedService(_configuration);
        //services.AddScoped<ICategorieCacheService, CategorieCacheService>();
        //services.AddScoped<ILangueCacheService, LangueCacheService>();

        ////Messaging. 
        //services.AddRedisMessaging();

        ////Mail. 
        ////services.AddMailSmtp(_configuration);
        //services.AddMailMailjet(_configuration);

        ////Strategies
        //services.AddScoped<ICrudStrategy, LangueStrategy>();
        //services.AddScoped<ICrudStrategy, CategorieStrategy>();

        ////Business
        //services.AddHostedService<SampleBackgroundService>();
        //services.AddScoped<ILogicielCsvService, LogicielCsvService>();
        //services.AddScoped<ILogicielCsvDataService, LogicielCsvDataService>();
        //services.AddScoped<ICsvReadService, CsvReadService>();

        ////Auth
        //services.AddScoped<IAuthService, AuthService>();
    }
}