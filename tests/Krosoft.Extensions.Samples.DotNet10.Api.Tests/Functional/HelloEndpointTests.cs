using System.Net;
using Krosoft.Extensions.Samples.DotNet10.Api.Tests.Core;
using Krosoft.Extensions.Samples.DotNet9.Api;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Tests.Functional;

//[TestClass]
//public class HelloEndpointTests : SampleBaseApiTest<Program>
//{
//    [TestMethod]
//    public async Task Hello_Ok()
//    {
//        var httpClient = Factory.CreateClient();
//        var response = await httpClient.GetAsync("/Hello");

//        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
//        var result = await response.Content.ReadAsStringAsync(CancellationToken.None);
//        Check.That(result).IsEqualTo("Hello DotNet9");
//    }

//    [TestMethod]
//    public async Task XForwardedPrefixHeader_SetPathBase_Correctly()
//    {
//        string? capturedPathBase = null;
//        var server = InitServer(capturedPathBase);

//        var httpClient = server.CreateClient();
//        httpClient.DefaultRequestHeaders.Add("X-Forwarded-Prefix", "/myprefix");
//        var response = await httpClient.GetAsync("/Hello");

//        Check.That(capturedPathBase).IsEqualTo("/myprefix");
//        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
//    }

//    [TestMethod]
//    public async Task XForwardedPrefixHeader_NotSet_PathBase_RemainsEmpty()
//    {

//        string? capturedPathBase = null;
//        var server = InitServer(capturedPathBase);

//        var httpClient = server.CreateClient();
//        var response = await httpClient.GetAsync("/Hello");

//        Check.That(capturedPathBase).IsEqualTo(string.Empty);
//        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
//    }

//    private static TestServer     InitServer(  string? capturedPathBase)
//    {
//        var mockEnvironment = new Mock<IWebHostEnvironment>();
//        mockEnvironment.Setup(e => e.EnvironmentName).Returns("Development");
//        mockEnvironment.Setup(e => e.ApplicationName).Returns("TestApp");
//        mockEnvironment.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

//        var configurationRoot = new ConfigurationBuilder()
//                                .SetBasePath(Directory.GetCurrentDirectory())
//                                .Build();

//        var builder = new WebHostBuilder()
//                      .ConfigureServices(services => { services.AddWebApi(configurationRoot); })
//                      .Configure(app =>
//                      {
//                          app.UseWebApi(mockEnvironment.Object, configurationRoot);

//                          app.Use(async (context, next) =>
//                          {
//                              capturedPathBase = context.Request.PathBase.Value;
//                              await next();
//                          });
//                      });

//        var server = new TestServer(builder);
//        return server;
//    }
//}

[TestClass]
public class HelloEndpointTests : SampleBaseApiTest<Program>
{
    [TestMethod]
    public async Task Hello_Ok()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.GetAsync("/Hello");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync(CancellationToken.None);
        Check.That(result).IsEqualTo("Hello DotNet9");
    }

    [TestMethod]
    public async Task XForwardedPrefixHeader_SetPathBase_Correctly()
    {
        string? capturedPathBase = null;
        var httpClient = InitializeClient("/myprefix", s => { capturedPathBase = s; });
        var response = await httpClient.GetAsync("/Hello");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
        Check.That(capturedPathBase).IsEqualTo("/myprefix");
    }

    [TestMethod]
    public async Task XForwardedPrefixHeader_NotSet_PathBase_RemainsEmpty()
    {
        string? capturedPathBase = null;
        var httpClient = InitializeClient(null, s => { capturedPathBase = s; });
        var response = await httpClient.GetAsync("/Hello");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
        Check.That(capturedPathBase).IsEqualTo(string.Empty);
    }

    private static HttpClient InitializeClient(string? prefix, Action<string?> check)
    {
        var mockEnvironment = new Mock<IWebHostEnvironment>();
        mockEnvironment.Setup(e => e.EnvironmentName).Returns("Development");
        mockEnvironment.Setup(e => e.ApplicationName).Returns("TestApp");
        mockEnvironment.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

        var configurationRoot = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .Build();

        var builder = new WebHostBuilder()
                      .ConfigureServices(services => { services.AddWebApi(configurationRoot); })
                      .Configure(app =>
                      {
                          app.UseWebApi(mockEnvironment.Object, configurationRoot);

                          app.Use(async (context, next) =>
                          {
                              var capturedPathBase = context.Request.PathBase.Value;
                              check(capturedPathBase);
                              await next();
                          });
                      });

        var server = new TestServer(builder);
        var httpClient = server.CreateClient();

        if (!string.IsNullOrEmpty(prefix))
        {
            httpClient.DefaultRequestHeaders.Add("X-Forwarded-Prefix", prefix);
        }

        return httpClient;
    }
}