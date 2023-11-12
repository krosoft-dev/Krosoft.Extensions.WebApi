using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Testing;

public abstract class BaseTest
{
    protected BaseTest()
    {
        SetDefaultCulture();
    }

    private static IConfigurationRoot GetConfiguration() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .Build();

    protected static void SetDefaultCulture()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-fr");
    }

    protected ServiceProvider CreateServiceCollection(Action<IServiceCollection> action = null)
    {
        var services = new ServiceCollection();

        var configuration = GetConfiguration();
        services.AddSingleton<IConfiguration>(configuration);
        AddServices(services, configuration);

        if (action != null)
        {
            action(services);
        }

        return services.BuildServiceProvider();
    }

    protected virtual void AddServices(ServiceCollection services, IConfiguration configuration)
    {
    }
}