using Krosoft.Extensions.Samples.Library.Models.Queries;
using Krosoft.Extensions.Testing.Cqrs.Extensions;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Tests.Unit.Handlers.Queries;

[TestClass]
public class HelloWorlQueryHandlerTests : SampleBaseTest<Startup>
{
    [TestMethod]
    public async Task Handle_Ok()
    {
        var serviceProvider = CreateServiceCollection();

        var result = await this.SendQueryAsync(serviceProvider, new HelloDotNet8Query());
        Check.That(result).IsNotNull();
        Check.That(result).IsEqualTo("Hello DotNet8");
    }
}