using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Core.Services;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Models;
using Krosoft.Extensions.Identity.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Krosoft.Extensions.Identity.Tests.Services;

[TestClass]
public class TokenProviderTests
{
    [TestMethod]
    public void GenerateTokenDifferentTest()
    {
        var purpose = "test";
        var securityStamp = Guid.NewGuid().ToString();
        var identifier = Guid.NewGuid().ToString();

        var tokenProvider = GetTokenProvider(false);
        var token1 = tokenProvider.GenerateToken(purpose, securityStamp, identifier);
        Check.That(token1).IsNotNull();

        var token2 = tokenProvider.GenerateToken(purpose, securityStamp, identifier);
        Check.That(token2).IsNotNull();

        Check.That(token1 != token2).IsTrue();
    }

    [TestMethod]
    public void GenerateTokenKoExpiredTest()
    {
        var purpose = "test";
        var securityStamp = Guid.NewGuid().ToString();
        var identifier = Guid.NewGuid().ToString();

        var tokenProviderWithMock = GetTokenProvider(true);
        var token = tokenProviderWithMock.GenerateToken(purpose, securityStamp, identifier);
        Check.That(token).IsNotNull();

        var tokenProvider = GetTokenProvider(false);
        var isValid = tokenProvider.Validate(purpose, securityStamp, identifier, token);
        Check.That(isValid).IsFalse();
    }

    [TestMethod]
    public void GenerateTokenKoIdentifierTest()
    {
        var purpose = "test";
        var securityStamp = Guid.NewGuid().ToString();
        var identifier1 = Guid.NewGuid().ToString();
        var identifier2 = Guid.NewGuid().ToString();

        var tokenProvider = GetTokenProvider(false);
        var token = tokenProvider.GenerateToken(purpose, securityStamp, identifier1);
        Check.That(token).IsNotNull();

        var isValid = tokenProvider.Validate(purpose, securityStamp, identifier2, token);
        Check.That(isValid).IsFalse();
    }

    [TestMethod]
    public void GenerateTokenKoPurposeTest()
    {
        var purpose1 = "test1";
        var purpose2 = "test2";
        var securityStamp = Guid.NewGuid().ToString();
        var identifier = Guid.NewGuid().ToString();

        var tokenProvider = GetTokenProvider(false);
        var token = tokenProvider.GenerateToken(purpose1, securityStamp, identifier);
        Check.That(token).IsNotNull();

        var isValid = tokenProvider.Validate(purpose2, securityStamp, identifier, token);
        Check.That(isValid).IsFalse();
    }

    [TestMethod]
    public void GenerateTokenKoSecurityStampTest()
    {
        var purpose = "test";
        var securityStamp1 = Guid.NewGuid().ToString();
        var securityStamp2 = Guid.NewGuid().ToString();
        var identifier = Guid.NewGuid().ToString();

        var tokenProvider = GetTokenProvider(false);
        var token = tokenProvider.GenerateToken(purpose, securityStamp1, identifier);
        Check.That(token).IsNotNull();

        var isValid = tokenProvider.Validate(purpose, token, securityStamp2, identifier);
        Check.That(isValid).IsFalse();
    }

    [TestMethod]
    public void GenerateTokenOkTest()
    {
        var purpose = "test";
        var securityStamp = Guid.NewGuid().ToString();
        var identifier = Guid.NewGuid().ToString();

        var tokenProvider = GetTokenProvider(false);
        var token = tokenProvider.GenerateToken(purpose, securityStamp, identifier);
        Check.That(token).IsNotNull();

        var isValid = tokenProvider.Validate(purpose, securityStamp, identifier, token);
        Check.That(isValid).IsTrue();
    }

    private static ITokenProvider GetTokenProvider(bool mockDateTime)
    {
        var services = new ServiceCollection();
        if (mockDateTime)
        {
            var mock = new Mock<IDateTimeService>();
            mock.Setup(x => x.Now).Returns(DateTime.Now.AddDays(-5));
            services.AddTransient(_ => mock.Object);
        }
        else
        {
            services.AddTransient<IDateTimeService, DateTimeService>();
        }

        services.AddTransient<ITokenProvider, TokenProvider>();
        services.AddDataProtection();

        services.AddTransient(_ => Options.Create(new TokenSettings
        {
            TokenLifespan = 2
        }));
        var serviceProvider = services.BuildServiceProvider();
        var tokenProvider = serviceProvider.GetRequiredService<ITokenProvider>();
        return tokenProvider;
    }
}