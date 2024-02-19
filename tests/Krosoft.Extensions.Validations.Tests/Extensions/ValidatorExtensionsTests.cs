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
public class ValidatorExtensionsTests : BaseTest
{
    private IValidator<SampleEntity> _validator = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddWebApi(configuration, typeof(SampleEntity).Assembly);
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _validator = serviceProvider.GetRequiredService<IValidator<SampleEntity>>();

        Check.That(_validator).IsNotNull();
    }

    [TestMethod]
    public void ValidateAndThrowAsync_Ok()
    {
        Check.ThatCode(() => _validator.ValidateMoreAndThrowAsync(new SampleEntity(), CancellationToken.None))
             .Throws<KrosoftFunctionalException>()
             .WithMessage("'Id' ne doit pas être vide.");
    }

    [TestMethod]
    public async Task ValidateMoreAsync_Empty()
    {
        IEnumerable<string>? f = null;
        await _validator.ValidateMoreAsync(new SampleEntity
        {
            Id = 1,
            Name = "Herllo"
        }, failures => { f = failures; }, CancellationToken.None);

        Check.That(f).IsEmpty();
    }

    [TestMethod]
    public async Task ValidateMoreAsync_Ok()
    {
        IEnumerable<string>? f = null;
        await _validator.ValidateMoreAsync(new SampleEntity(), failures => { f = failures; }, CancellationToken.None);

        Check.That(f).HasSize(3);
        Check.That(f).ContainsExactly("'Id' ne doit pas être vide.", "'Name' ne doit pas être vide.", "'Name' ne doit pas avoir la valeur null.");
    }
}