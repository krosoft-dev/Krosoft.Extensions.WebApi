using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Extensions;
using Krosoft.Extensions.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Identity.Tests.Services;

[TestClass]
public class SimplePasswordHasherTests : BaseTest
{
    private const string HashString = "jgfPcJM+rUjk4EKjyrQXY/Xm9vaxPlK/g4jflQZU11d++xqVhfXCMV4SVd1T+QeU6lFQ1tdul8xZt34DhdIsbw==";
    private const string Password = "password";
    private const string SaltString = "4j3KMAwlAA82fJ4d90EUhL8NVOOoj+22wLY1gX/I8/befgVogSNZnVnU8w8IifbzzTVF7ptTZ5Vf8VZiz6gVP3ZudkI5KWdwq/6/iTWCcY+5AG6nE/z4DTgIoz6yHAw1JIiSkWNUXgglsSHFqRfvUiW1nBlsQC7KdWAgjyQJgqU=";
    private ISimplePasswordHasher? _passwordHasher;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityEx();
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _passwordHasher = serviceProvider.GetRequiredService<ISimplePasswordHasher>();
    }

    [TestMethod]
    public void VerifyOkTest()
    {
        var hash = Convert.FromBase64String(HashString);
        var salt = Convert.FromBase64String(SaltString);
        var isOk = _passwordHasher!.Verify(Password, hash, salt);
        Check.That(isOk).IsTrue();
    }

    [TestMethod]
    public void VerifyKoTest()
    {
        var hash = Convert.FromBase64String(HashString);
        var salt = Convert.FromBase64String(SaltString);
        var isOk = _passwordHasher!.Verify("password1", hash, salt);
        Check.That(isOk).IsFalse();
    }

    [TestMethod]
    public void CreatePasswordHashTest()
    {
        var hashSalt = _passwordHasher!.CreatePasswordHash(Password);
        Check.That(hashSalt).IsNotNull();
        Check.That(hashSalt.Hash).IsNotNull();
        Check.That(hashSalt.Salt).IsNotNull();

        var isOk = _passwordHasher.Verify(Password, hashSalt.Hash, hashSalt.Salt);
        Check.That(isOk).IsTrue();
    }
}