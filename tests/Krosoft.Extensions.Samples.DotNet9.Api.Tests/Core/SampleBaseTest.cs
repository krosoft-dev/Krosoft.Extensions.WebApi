using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Cqrs.Behaviors.Extensions;
using Krosoft.Extensions.Cqrs.Behaviors.Validations.Extensions;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

//using Moq;
//using Krosoft.Extensions.Application.Extensions;
//using Krosoft.Extensions.Data.EntityFramework.Extensions;
//using Krosoft.Extensions.Data.EntityFramework.InMemory.Extensions;
//using Krosoft.Extensions.Identity.Abstractions.Interfaces;
//using Krosoft.Extensions.Infrastructure.Extensions;
//using Krosoft.Extensions.Samples.Api.Data;
//using Krosoft.Extensions.Testing;
//using Krosoft.Extensions.Testing.Extensions;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Tests.Core;

public abstract class SampleBaseTest<TEntry> : BaseTest
{
    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        var executingAssembly = typeof(TEntry).Assembly;

        services.AddWebApi(configuration, executingAssembly)
                .AddDateTimeService()  .AddBehaviors(options => options.AddLogging()
                                                                       .AddValidations());

        base.AddServices(services, configuration);
    }
}