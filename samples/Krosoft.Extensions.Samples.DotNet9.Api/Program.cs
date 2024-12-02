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
using Krosoft.Extensions.Samples.DotNet9.Api.Data;
using Krosoft.Extensions.Samples.Library.Mappings;
using Krosoft.Extensions.WebApi.Blocking.Extensions;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.HealthChecks.Extensions;
using Krosoft.Extensions.WebApi.Identity.Extensions;
using Krosoft.Extensions.WebApi.Swagger.Extensions;
using Krosoft.Extensions.WebApi.Swagger.HealthChecks.Extensions;
using Krosoft.Extensions.Zip.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var currentAssembly = Assembly.GetExecutingAssembly();

var builder = WebApplication.CreateBuilder(args);

//Web API.
builder.Services
       .AddWebApi(builder.Configuration, currentAssembly, typeof(CompteProfile).Assembly)
       //CQRS.
       .AddBehaviors(options => options.AddLogging()
                                       .AddValidations()
                                       .AddIdentity())
       //Swagger.
       .AddSwagger(currentAssembly, options => options.AddHealthChecks()
                                                      .AddGlobalResponses()
                                                      .AddSecurityBearer()
                                                      .AddSecurityApiKey())
       //Blocking.
       .AddBlocking()
       .AddWepApiBlocking()
       .AddMemoryBlockingStorage()

//Identity.
       .AddIdentityEx()
       .AddWebApiIdentityEx()

//Data.
       .AddRepositories()
       .AddDbContextInMemory<SampleKrosoftContext>(false)
//services.AddDbContextSqlite<SampleKrosoftContext>(_configuration); 
//services.AddDbContextPostgreSql<KrosoftExtensionTenantContext>(_configuration);
       .AddSeedService<SampleKrosoftContext, SampleKrosoftContextSeedService>()

//Cache. 
       .AddDistributedCacheExt()

//Autres
       .AddZip()
       .AddPdf()
       .AddCorsPolicyAccessor()
       .AddHealthChecks()
       .AddCheck("Test_Endpoint", () => HealthCheckResult.Healthy())
       .AddRedisCheck()
       .AddDbContextCheck<SampleKrosoftContext>("SampleKrosoftContext")
    ;

//builder.Services
//       .AddVigilantus(builder.Configuration)
//       .AddWebApi(builder.Configuration, typeof(ServiceCollectionExtensions).Assembly)
//       .AddSwagger(currentAssembly, options => options.AddHealthChecks()
//                                                      .AddSecurityBearer()
//                                                      .AddGlobalResponses())
//       .AddBehaviors(options => options.AddLogging()
//                                       .AddValidations()
//                    )

//       //Data.
//       .AddRepositories()
//       .AddDbContextSqlServer<EdiCoreContext>(builder.Configuration)
//       .AddHealthChecks();

var app = builder.Build();
app.UseWebApi(builder.Environment, builder.Configuration,
              x => x.UseHealthChecksExt(builder.Environment),
              endpoints => endpoints.MapHealthChecksExt())
   .UseSwaggerExt()
   .UseBlocking();

await app.RunAsync();

namespace Krosoft.Extensions.Samples.DotNet9.Api
{
    public class Program;
}