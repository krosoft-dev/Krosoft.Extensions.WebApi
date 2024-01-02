using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.InMemory.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Models;
using Krosoft.Extensions.Samples.DotNet8.Api.Data;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.Tests.Extensions;

[TestClass]
public class ServiceProviderExtensionsTests : BaseTest
{
    [TestMethod]
    public async Task CreateDbContextScope_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddRepositories();
            services.AddDbContextInMemory<SampleKrosoftContext>(true);
            services.AddSeedService<SampleKrosoftContext, SampleSeedService<SampleKrosoftContext>>();
        }

        await using var serviceProvider = CreateServiceCollection(GetServices);
        using var contextScope = serviceProvider.CreateDbContextScope(new DbContextSettings<SampleKrosoftContext>());

        var repository = contextScope.GetWriteRepository<Logiciel>();

        var logiciels = await repository.Query()
                                        .ToListAsync(CancellationToken.None);

        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(5);
        Check.That(logiciels.Select(x => x.Nom)).ContainsExactly("Logiciel1", "Logiciel2", "Logiciel3", "Logiciel4", "Logiciel5");
    }

    [TestMethod]
    public async Task CreateReadDbContextScope_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddRepositories();
            services.AddDbContextInMemory<SampleKrosoftContext>(true);
            services.AddSeedService<SampleKrosoftContext, SampleSeedService<SampleKrosoftContext>>();
        }

        await using var serviceProvider = CreateServiceCollection(GetServices);
        using var contextScope = serviceProvider.CreateReadDbContextScope(new DbContextSettings<SampleKrosoftContext>());

        var repository = contextScope.GetReadRepository<Logiciel>();

        var logiciels = await repository.Query()
                                        .ToListAsync(CancellationToken.None);

        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(5);
        Check.That(logiciels.Select(x => x.Nom)).ContainsExactly("Logiciel1", "Logiciel2", "Logiciel3", "Logiciel4", "Logiciel5");
    }
}