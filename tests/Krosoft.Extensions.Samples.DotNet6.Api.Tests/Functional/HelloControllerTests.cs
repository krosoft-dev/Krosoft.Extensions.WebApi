using System.Net;
using Krosoft.Extensions.Samples.DotNet6.Api.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Tests.Functional;

[TestClass]
public class HelloControllerTests : SampleBaseApiTest<Startup>
{
    [TestMethod]
    public async Task Hello_Ok()
    {
        var url = "/Hello";

        var httpClient = Factory.CreateClient();
        var response = await httpClient.GetAsync(url);

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

        var result = await response.Content.ReadAsStringAsync(CancellationToken.None);
        Check.That(result).IsEqualTo("Hello World");
    }
}