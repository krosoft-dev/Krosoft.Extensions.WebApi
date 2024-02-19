using FluentValidation;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.Validations.Extensions;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Validations.Tests.Extensions;

[TestClass]
public class ValidatorsExtensionsTests : BaseTest
{
    private IEnumerable<IValidator<SampleEntity>> _validators = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddWebApi(configuration, typeof(SampleEntity).Assembly);
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _validators = serviceProvider.GetRequiredService<IEnumerable<IValidator<SampleEntity>>>();

        Check.That(_validators).HasSize(1);
    }

    [TestMethod]
    public async Task ValidateAsync_Empty()
    {
        var failures = await _validators.ValidateMoreAsync(new SampleEntity
        {
            Id = 1,
            Name = "Herllo"
        }, CancellationToken.None);

        Check.That(failures).IsEmpty();
    }

    [TestMethod]
    public async Task ValidateAsync_Ok()
    {
        var failures = await _validators.ValidateMoreAsync(new SampleEntity(), CancellationToken.None);

        Check.That(failures).HasSize(3);
        Check.That(failures).ContainsExactly("'Id' ne doit pas être vide.", "'Name' ne doit pas être vide.", "'Name' ne doit pas avoir la valeur null.");
    }

    [TestMethod]
    public void ValidateAndThrowAsync_Ok()
    {
        Check.ThatCode(() => _validators.ValidateMoreAndThrowAsync(new SampleEntity(), CancellationToken.None))
             .Throws<KrosoftFunctionalException>()
             .WithMessage("'Id' ne doit pas être vide.");
    }
}