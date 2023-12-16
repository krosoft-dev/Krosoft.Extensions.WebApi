using System.Net;
using HealthChecks.UI.Core;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.DotNet6.Api.Tests.Core;
using Krosoft.Extensions.WebApi.HealthChecks.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Tests.Functional;

[TestClass]
public class HealthCheckTests : SampleBaseApiTest<Startup>
{
    [TestMethod]
    public async Task HealthCheck_Ok()
    {
        Factory = GetFactory();
        var client = Factory.CreateClient();
        var response = await client.GetAsync("/Health/Check");
        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var model = await response.Content.ReadAsJsonAsync<HealthCheckStatusModel>(CancellationToken.None);

        Check.That(model).IsNotNull();
        Check.That(model!.Status).IsEqualTo("Healthy");
        Check.That(model.Duration).IsNotEmpty();
        Check.That(model.Checks).IsNotNull();
        var checks = model.Checks.OrderBy(c => c.Key).ToList();
        Check.That(checks).HasSize(2);
        Check.That(checks.Select(c => c.Key)).ContainsExactly("self", "Test_Endpoint");
        Check.That(checks.Select(c => c.Status)).ContainsExactly("Healthy", "Healthy");
        Check.That(checks.Select(c => c.Description)).ContainsExactly(null, null);
        //Check.That(checks).HasSize(4);
        //Check.That(checks.Select(c => c.Key)).ContainsExactly("PositiveExtensionTenantContext", "Redis", "self", "test");
        //Check.That(checks.Select(c => c.Status)).ContainsExactly("Healthy", "Healthy", "Healthy", "Healthy");
        //Check.That(checks.Select(c => c.Description)).ContainsExactly(null, "Ping Redis en 0ms", null, null);
    }

    [TestMethod]
    public async Task HealthReadiness_Ok()
    {
        Factory = GetFactory();
        var client = Factory.CreateClient();
        var response = await client.GetAsync("/Health/Readiness");
        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var model = await response.Content.ReadAsJsonAsync<UIHealthReport>(CancellationToken.None);

        Check.That(model).IsNotNull();
        Check.That(model!.Status).IsEqualTo(UIHealthStatus.Healthy);
        Check.That(model.TotalDuration).IsLessThan(TimeSpan.FromSeconds(1));

        Check.That(model.Entries).IsNotNull();
        var entries = model.Entries.OrderBy(c => c.Key).ToList();
        Check.That(entries).HasSize(2);
        Check.That(entries.Select(c => c.Key)).ContainsExactly("self", "Test_Endpoint");
        Check.That(entries.Select(c => c.Value.Status)).ContainsExactly(UIHealthStatus.Healthy, UIHealthStatus.Healthy);
        //Check.That(entries).HasSize(4);
        //Check.That(entries.Select(c => c.Key)).ContainsExactly("PositiveExtensionTenantContext", "Redis", "self", "test");
        //Check.That(entries.Select(c => c.Value.Status)).ContainsExactly(UIHealthStatus.Healthy, UIHealthStatus.Healthy, UIHealthStatus.Healthy, UIHealthStatus.Healthy);
    }

    [TestMethod]
    public async Task HealthLiveness_Ok()
    {
        Factory = GetFactory();
        var client = Factory.CreateClient();
        var response = await client.GetAsync("/Health/Liveness");
        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var model = await response.Content.ReadAsJsonAsync<UIHealthReport>(CancellationToken.None);

        Check.That(model).IsNotNull();
        Check.That(model!.Status).IsEqualTo(UIHealthStatus.Healthy);
        Check.That(model.TotalDuration).IsLessThan(TimeSpan.FromSeconds(1));

        Check.That(model.Entries).IsNotNull();
        var entries = model.Entries.OrderBy(c => c.Key).ToList();
        Check.That(entries).HasSize(1);
        Check.That(entries.Select(c => c.Key)).ContainsExactly("self");
        Check.That(entries.Select(c => c.Value.Status)).ContainsExactly(UIHealthStatus.Healthy);
    }
}