using System.Net;
using Krosoft.Extensions.Samples.DotNet6.Api.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Tests.Functional;

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