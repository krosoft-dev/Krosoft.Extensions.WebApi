using System.Net;
using Krosoft.Extensions.Samples.DotNet8.Api.Tests.Core;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Tests.Functional;

[TestClass]
public class ControllerTests : SampleBaseApiTest<Startup>
{
    [TestMethod]
    public async Task IndexTest()
    {
        var response = await Factory.CreateClient().GetAsync("/");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task RandomEndpointTest()
    {
        var response = await Factory.CreateClient().GetAsync("/azerty");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }
}