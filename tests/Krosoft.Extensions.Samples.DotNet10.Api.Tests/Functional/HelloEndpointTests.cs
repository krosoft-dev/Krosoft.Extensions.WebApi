using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Samples.DotNet10.Api.Tests.Core;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Json;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Tests.Functional;

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
        Check.That(result).IsEqualTo("Hello DotNet10");
    }

    [TestMethod]
    public async Task Hello_Ok_BadRequest()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.PostAsJsonAsync("/Hello", new object(), CancellationToken.None);

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadAsNewtonsoftJsonAsync<ErrorDto>();
        Check.That(error).IsNotNull();
        Check.That(error!.Code).IsEqualTo(400);
        Check.That(error.Errors)
             .ContainsExactly("'Name' ne doit pas être vide.");
    }

    [TestMethod]
    public async Task XForwardedPrefixHeader_SetPathBase_Correctly()
    {
        string? capturedPathBase = null;
        var httpClient = await InitializeClientAsync("/myprefix", s => { capturedPathBase = s; });
        var response = await httpClient.GetAsync("/Hello");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
        Check.That(capturedPathBase).IsEqualTo("/myprefix");
    }

    [TestMethod]
    public async Task XForwardedPrefixHeader_NotSet_PathBase_RemainsEmpty()
    {
        string? capturedPathBase = null;
        var httpClient = await InitializeClientAsync(null, s => { capturedPathBase = s; });
        var response = await httpClient.GetAsync("/Hello");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
        Check.That(capturedPathBase).IsEqualTo(string.Empty);
    }

    private static async Task<HttpClient> InitializeClientAsync(string? prefix, Action<string?> check)
    {
        var mockEnvironment = new Mock<IWebHostEnvironment>();
        mockEnvironment.Setup(e => e.EnvironmentName).Returns("Development");
        mockEnvironment.Setup(e => e.ApplicationName).Returns("TestApp");
        mockEnvironment.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

        var configurationRoot = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .Build();

        var builder = WebApplication.CreateBuilder();
        builder.Services.AddWebApi(configurationRoot);
        builder.WebHost.UseTestServer();
        var app = builder.Build();
        app.UseWebApi(mockEnvironment.Object, configurationRoot);
        app.Use(async (context, next) =>
        {
            var capturedPathBase = context.Request.PathBase.Value;
            check(capturedPathBase);
            await next();
        });
        await app.StartAsync();
        var httpClient = app.GetTestClient();

        if (!string.IsNullOrEmpty(prefix))
        {
            httpClient.DefaultRequestHeaders.Add("X-Forwarded-Prefix", prefix);
        }

        return httpClient;
    }
}