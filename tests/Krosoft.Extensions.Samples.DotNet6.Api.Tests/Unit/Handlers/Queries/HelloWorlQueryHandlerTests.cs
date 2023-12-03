using Krosoft.Extensions.Samples.DotNet6.Api.Models.Queries;
using Krosoft.Extensions.Samples.DotNet6.Api.Tests.Core;
using Krosoft.Extensions.Testing.Cqrs.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Tests.Unit.Handlers.Queries;

[TestClass]
public class HelloWorlQueryHandlerTests : SampleBaseTest<Startup>
{
    [TestMethod]
    public async Task HandleEmptyTest()
    {
        var serviceProvider = CreateServiceCollection();

        var result = await this.SendQueryAsync(serviceProvider, new HelloWorlQuery());
        Check.That(result).IsNotNull();
        Check.That(result).IsEqualTo("Hello World");
    }
}