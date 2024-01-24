using JetBrains.Annotations;
using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.Blocking.Extensions;
using Krosoft.Extensions.Blocking.Memory.Extensions;
using Krosoft.Extensions.Blocking.Services;
using Krosoft.Extensions.Identity.Abstractions.Fakes;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Blocking.Tests.Services;

[TestClass]
[TestSubject(typeof(IdentifierBlockingService))]
public class IdentifierBlockingServiceTests : BaseTest
{
    private IIdentifierBlockingService _identifierBlockingService = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLoggingExt();
        services.AddBlocking();
        services.AddMemoryBlockingStorage();
        services.AddTransient<IIdentifierProvider, FakeIdentifierProvider>();
    }

    [TestMethod]
    public async Task BlockAsync_Ok()
    {
        await _identifierBlockingService.BlockAsync(CancellationToken.None);
        var isBlocked = await _identifierBlockingService.IsBlockedAsync(CancellationToken.None);

        Check.That(isBlocked).IsTrue();
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _identifierBlockingService = serviceProvider.GetRequiredService<IIdentifierBlockingService>();
    }
}