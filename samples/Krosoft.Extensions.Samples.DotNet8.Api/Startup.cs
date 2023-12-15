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
using Krosoft.Extensions.Pdf.Extensions;
using Krosoft.Extensions.Samples.Library.Mappings;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.HealthChecks.Extensions;
using Krosoft.Extensions.WebApi.Swagger.Extensions;
using Krosoft.Extensions.WebApi.Swagger.HealthChecks.Extensions;
using Krosoft.Extensions.Zip.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Krosoft.Extensions.Samples.DotNet8.Api;

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
        app.UseWebApi(env, _configuration,
                      builder => builder.UseHealthChecksExt(env),
                      endpoints => endpoints.MapHealthChecksExt())
           .UseSwaggerExt();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        services.AddWebApi(_configuration, currentAssembly, typeof(CompteProfile).Assembly)
                .AddSwagger(currentAssembly, options => options.AddHealthChecks().AddSecurityBearer().AddSecurityApiKey());

        //
        //services.AddApplication(Assembly.GetExecutingAssembly());
        //services.AddInfrastructure(_configuration);
        //services.AddJwtAuthentication(_configuration).AddBlocking(_configuration);

        services.AddHealthChecks()
                .AddCheck("Test_Endpoint", () => HealthCheckResult.Healthy())
            //        .AddRedisCheck()
            //        .AddDbContextCheck<KrosoftExtensionTenantContext>("KrosoftExtensionTenantContext")
            ;

        services.AddZip();
        services.AddPdf();

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