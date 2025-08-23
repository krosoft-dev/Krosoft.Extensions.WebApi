using System.Reflection;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Cqrs.Behaviors.Extensions;
using Krosoft.Extensions.Cqrs.Behaviors.Validations.Extensions;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.HealthChecks.Extensions;
using Krosoft.Extensions.WebApi.Swagger.Extensions;
using Krosoft.Extensions.WebApi.Swagger.HealthChecks.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var currentAssembly = Assembly.GetExecutingAssembly();

var assemblies = new[]
{
    currentAssembly
};

var builder = WebApplication.CreateBuilder(args);

//Web API.
builder.Services
       .AddDateTimeService()
       .AddWebApi(builder.Configuration, assemblies)
       //CQRS.
       .AddBehaviors(options => options.AddLogging()
                                       .AddValidations())
       //Swagger.
       .AddSwagger(currentAssembly, options => options.AddHealthChecks()
                                                      .AddGlobalResponses())

//Autres
       .AddHealthChecks()
       .AddCheck("Test_Endpoint", () => HealthCheckResult.Healthy())
    ;

var app = builder.Build();
app.UseWebApi(builder.Environment, builder.Configuration,
              x => x.UseHealthChecksExt(builder.Environment), endpoints => endpoints.MapHealthChecksExt())
   .UseSwaggerExt();

await app
      .AddEndpoints(currentAssembly)
      .RunAsync();

namespace Krosoft.Extensions.Samples.DotNet9.Api
{
    public class Program;
}