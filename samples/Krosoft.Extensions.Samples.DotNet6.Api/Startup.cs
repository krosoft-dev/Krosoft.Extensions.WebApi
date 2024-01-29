using System.Reflection;
using Krosoft.Extensions.Blocking.Extensions;
using Krosoft.Extensions.Blocking.Memory.Extensions;
using Krosoft.Extensions.Cache.Distributed.Redis.Extensions;
using Krosoft.Extensions.Cache.Distributed.Redis.HealthChecks.Extensions;
using Krosoft.Extensions.Cqrs.Behaviors.Extensions;
using Krosoft.Extensions.Cqrs.Behaviors.Identity.Extensions;
using Krosoft.Extensions.Cqrs.Behaviors.Validations.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.InMemory.Extensions;
using Krosoft.Extensions.Identity.Extensions;
using Krosoft.Extensions.Pdf.Extensions;
using Krosoft.Extensions.Samples.DotNet6.Api.Data;
using Krosoft.Extensions.Samples.Library.Mappings;
using Krosoft.Extensions.WebApi.Blocking.Extensions;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.HealthChecks.Extensions;
using Krosoft.Extensions.WebApi.Identity.Extensions;
using Krosoft.Extensions.WebApi.Swagger.Extensions;
using Krosoft.Extensions.WebApi.Swagger.HealthChecks.Extensions;
using Krosoft.Extensions.Zip.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
        app.UseWebApi(env, _configuration,
                      builder => builder.UseHealthChecksExt(env),
                      endpoints => endpoints.MapHealthChecksExt())
           .UseSwaggerExt()
           .UseBlocking();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks()
                .AddCheck("Test_Endpoint", () => HealthCheckResult.Healthy())
                .AddRedisCheck()
                .AddDbContextCheck<SampleKrosoftContext>("SampleKrosoftContext")
            ;

        var currentAssembly = Assembly.GetExecutingAssembly();

        //Web API.
        services.AddWebApi(_configuration, currentAssembly, typeof(CompteProfile).Assembly);

        //CQRS.
        services.AddBehaviors(options => options.AddLogging()
                                                .AddValidations()
                                                .AddIdentity());

        //Swagger.
        services.AddSwagger(currentAssembly, options => options.AddHealthChecks()
                                                               .AddSecurityBearer()
                                                               .AddSecurityApiKey());

        //Blocking.
        services.AddBlocking()
                .AddWepApiBlocking()
                .AddMemoryBlockingStorage();

        //Identity.
        services.AddIdentityEx().AddWebApiIdentityEx();

        //Data.
        services.AddRepositories();
        services.AddDbContextInMemory<SampleKrosoftContext>(false);
        //services.AddDbContextSqlite<SampleKrosoftContext>(_configuration); 
        //services.AddDbContextPostgreSql<KrosoftExtensionTenantContext>(_configuration);
        services.AddSeedService<SampleKrosoftContext, SampleKrosoftContextSeedService>();

        //Cache. 
        services.AddDistributedCacheExt();

        //Autres
        services.AddZip();
        services.AddPdf();
        services.AddCorsPolicyAccessor();
    }
}