using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.WebApi.Identity.Extensions;
using Krosoft.Extensions.WebApi.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.WebApi.Identity.Tests.Extensions;

[TestClass]
public class ServiceCollectionExtensionsTests
{
    private const string Authority = "https://demo.supabase.co/auth/v1";
    private const string Audience = "authenticated";

    private static IConfiguration BuildConfiguration(IDictionary<string, string?> values) =>
        new ConfigurationBuilder().AddInMemoryCollection(values).Build();

    [TestMethod]
    public void AddJwtAuthorityAuthentication_Ok()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["JwtAuthoritySettings:Authority"] = Authority,
            ["JwtAuthoritySettings:Audience"] = Audience
        });

        var services = new ServiceCollection();
        services.AddJwtAuthorityAuthentication(configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Les settings sont bindés depuis la section JwtAuthoritySettings.
        var settings = serviceProvider.GetRequiredService<IOptions<JwtAuthoritySettings>>().Value;
        Check.That(settings.Authority).IsEqualTo(Authority);
        Check.That(settings.Audience).IsEqualTo(Audience);

        // Le schéma JwtBearer est enregistré.
        var schemeProvider = serviceProvider.GetRequiredService<IAuthenticationSchemeProvider>();
        var scheme = schemeProvider.GetSchemeAsync(JwtBearerDefaults.AuthenticationScheme).GetAwaiter().GetResult();
        Check.That(scheme).IsNotNull();

        // Les TokenValidationParameters reprennent l'autorité et l'audience configurées.
        var jwtBearerOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>()
                                              .Get(JwtBearerDefaults.AuthenticationScheme);
        Check.That(jwtBearerOptions.Authority).IsEqualTo(Authority);
        Check.That(jwtBearerOptions.TokenValidationParameters.ValidIssuer).IsEqualTo(Authority);
        Check.That(jwtBearerOptions.TokenValidationParameters.ValidAudience).IsEqualTo(Audience);
        Check.That(jwtBearerOptions.TokenValidationParameters.ValidateLifetime).IsTrue();
        Check.That(jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey).IsTrue();
    }

    [TestMethod]
    public void AddJwtAuthorityAuthentication_SectionAbsente()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>());

        var services = new ServiceCollection();
        Check.ThatCode(() => services.AddJwtAuthorityAuthentication(configuration))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Impossible d'instancier l'objet de type 'JwtAuthoritySettings'.");
    }
}
