using Krosoft.Extensions.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

//using Krosoft.Extensions.Cache.Distributed.Redis.Interfaces;
//using Krosoft.Extensions.Cache.Distributed.Redis.Services;
//using Krosoft.Extensions.Core.Extensions;
//using Krosoft.Extensions.Core.Models.Business;
//using Krosoft.Extensions.Core.Models.Exceptions;
//using Krosoft.Extensions.Data.EntityFramework.Contexts;
//using Krosoft.Extensions.Hosting.Services;
//using Krosoft.Extensions.Identity.Abstractions.Interfaces;
//using Krosoft.Extensions.Testing.AspNetCore.Services;
//using Krosoft.Extensions.Testing.AspNetCore.StartupFilters;
//using Krosoft.Extensions.Testing.Extensions;

namespace Krosoft.Extensions.Testing.WebApi;

/// <summary>
/// Factory pour les controleurs de base.
/// </summary>
/// <typeparam name="TStartup">Type du startup du projet à test.</typeparam>
/// <typeparam name="TKrosoftContext">DbContext du projet.</typeparam>
public class CustomWebApplicationFactory<TStartup, TKrosoftContext> : WebApplicationFactory<TStartup>
    where TStartup : class
//where TKrosoftContext : KrosoftContext
{
    private readonly Action<KrosoftToken>? _actionConfigureClaims;

    private readonly Action<IServiceCollection>? _actionConfigureServices;
    //private readonly bool _useFakeAuth;

    public CustomWebApplicationFactory(Action<IServiceCollection> actionConfigureServices,
                                       Action<KrosoftToken>? actionConfigureClaims,
                                       bool useFakeAuth)
    {
        _actionConfigureServices = actionConfigureServices;
        _actionConfigureClaims = actionConfigureClaims;
        //_useFakeAuth = useFakeAuth;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //services.AddSingleton<IStartupFilter, CustomStartupFilter>();
            //services.SwapTransient<IDistributedCacheProvider, DictionaryCacheProvider>();
            //services.RemoveServices(d => d.ImplementationType != null && d.ImplementationType.BaseType == typeof(InfiniteBackgroundService));
            //services.RemoveServices(d => d.ImplementationType != null && d.ImplementationType.BaseType == typeof(ScheduledHostedService));

            if (_actionConfigureServices != null)
            {
                _actionConfigureServices(services);
            }

            //if (_useFakeAuth)
            //{
            //    services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = FakeJwtTokenGenerator.SecurityKey,
            //            ValidateIssuer = true,
            //            ValidIssuer = FakeJwtTokenGenerator.Issuer,
            //            ValidateAudience = true,
            //            ValidAudience = FakeJwtTokenGenerator.Audience,
            //            ValidateLifetime = true,
            //            ClockSkew = TimeSpan.Zero
            //        };
            //    });
            //}
        });
    }

    public HttpClient CreateAuthenticatedClient()
    {
        var claimInfo = KrosoftTokenHelper.Defaut;
        if (_actionConfigureClaims != null)
        {
            _actionConfigureClaims(claimInfo);
        }

        return CreateAuthenticatedClient(claimInfo);
    }

    public HttpClient CreateAuthenticatedClient(KrosoftToken positiveToken)
    {
        var client = CreateClient();

        //var claims = GetClaims(positiveToken);

        //var token = FakeJwtTokenGenerator.CreateToken(claims);
        //client.SetBearerToken(token);

        return client;
    }

    //private IEnumerable<Claim> GetClaims(KrosoftToken positiveToken)
    //{
    //    var claimsService = Services.GetRequiredService<IClaimsBuilderService>();

    //    var claims = claimsService.Build(positiveToken).ToList();

    //    return claims;
    //}

    //public void WithService<T>(Action<T> action)
    //{
    //    var scopeFactory = Services.GetService<IServiceScopeFactory>();
    //    if (scopeFactory != null)
    //    {
    //        using (var scope = scopeFactory.CreateScope())
    //        {
    //            var service = scope.ServiceProvider.GetService<T>();

    //            action(service);
    //        }
    //    }
    //}

    public void WithService<T>(Action<T> action) where T : notnull
    {
        var scopeFactory = Services.GetRequiredService<IServiceScopeFactory>();

        using (var scope = scopeFactory.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<T>();

            action(service);
        }
    }

    //public List<T> Get<T>() where T : class
    //{
    //    var scopeFactory = Services.GetService<IServiceScopeFactory>();
    //    if (scopeFactory != null)
    //    {
    //        using (var scope = scopeFactory.CreateScope())
    //        {
    //            var context = scope.ServiceProvider.GetService<TKrosoftContext>();
    //            if (context != null)
    //            {
    //                var entities = context.Set<T>().ToList();

    //                return entities;
    //            }

    //            throw new KrosoftTechniqueException($"{nameof(TKrosoftContext)} introuvable.");
    //        }
    //    }

    //    throw new KrosoftTechniqueException($"{nameof(IServiceScopeFactory)} introuvable.");
    //}

    //public void Remove<T>(T entity)
    //{
    //    WithService<TKrosoftContext>(db =>
    //    {
    //        db.Remove(entity);
    //        db.SaveChanges();
    //    });
    //}

    //public void RemoveRange<T>() where T : class
    //{
    //    WithService<TKrosoftContext>(db =>
    //    {
    //        db.RemoveRange(db.Set<T>());
    //        db.SaveChanges();
    //    });
    //}
}