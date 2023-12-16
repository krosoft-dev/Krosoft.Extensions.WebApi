using AutoMapper;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Mapping.Extensions;
using Krosoft.Extensions.Samples.Library.Mappings;
using Krosoft.Extensions.Samples.Library.Models;
using Krosoft.Extensions.Samples.Library.Models.Dto;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Mapping.Tests.Extensions;

[TestClass]
public class MapperExtensionsTests : BaseTest
{
    private IMapper _mapper = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddWebApi(configuration, typeof(CompteProfile).Assembly);
    }

    private static IEnumerable<Compte> GetComptes()
    {
        for (var c = 'A'; c <= 'Z'; c++)
        {
            yield return new Compte
            {
                Id = $"{c}",
                Name = $"Compte {c}"
            };
        }
    }

    [TestMethod]
    public void MapIfExist_Action()
    {
        var source = new Compte { Name = "Test" };
        var destination = new CompteDto();

        _mapper.MapIfExist(source, destination, () => throw new KrosoftTechniqueException("Test"));

        Check.That(destination.Name).IsEqualTo("Test");
    }

    [TestMethod]
    public void MapIfExist_ActionNull()
    {
        Compte? source = null;
        var destination = new CompteDto();

        Check.ThatCode(() => _mapper.MapIfExist(source, destination, () => throw new KrosoftTechniqueException("Test")))
             .Throws<KrosoftTechniqueException>()
             .WithMessage("Test");
    }

    [TestMethod]
    public void MapIfExist_ConcurrentDictionary()
    {
        var comptesParId = GetComptes().ToConcurrentDictionary(x => x.Id!);

        var compteDto = new CompteDto();
        _mapper.MapIfExist(comptesParId, "K", compteDto);

        Check.That(compteDto.Name).IsEqualTo("Compte K");
    }

    [TestMethod]
    public void MapIfExist_Dictionary()
    {
        var comptesParId = GetComptes().ToDictionary(x => x.Id!);

        var compteDto = new CompteDto();
        _mapper.MapIfExist(comptesParId, "K", compteDto);

        Check.That(compteDto.Name).IsEqualTo("Compte K");
    }

    [TestMethod]
    public void MapIfExist_Null()
    {
        Compte? source = null;
        var destination = new CompteDto();

        _mapper.MapIfExist(source, destination);

        Check.That(destination.Name).IsNull();
    }

    [TestMethod]
    public void MapIfExist_Ok()
    {
        var source = new Compte { Name = "Test" };
        var destination = new CompteDto();

        _mapper.MapIfExist(source, destination);

        Check.That(destination.Name).IsEqualTo("Test");
    }

    [TestMethod]
    public void MapIfExist_ReadOnlyDictionary()
    {
        var comptesParId = GetComptes().ToReadOnlyDictionary(x => x.Id!);

        var compteDto = new CompteDto();
        _mapper.MapIfExist((IDictionary<string, Compte>)comptesParId, "K", compteDto);

        Check.That(compteDto.Name).IsEqualTo("Compte K");
    }

    [TestMethod]
    public void MapIfExist_ToType()
    {
        var source = new Compte { Name = "Test" };

        var destination = _mapper.MapIfExist<CompteDto>(source);

        Check.That(destination).IsNotNull();
        Check.That(destination!.Name).IsEqualTo("Test");
    }

    [TestMethod]
    public void MapIfExist_ToTypeAction()
    {
        var source = new Compte { Name = "Test" };

        var destination = _mapper.MapIfExist<CompteDto>(source, () => throw new KrosoftTechniqueException("Test"));

        Check.That(destination.Name).IsEqualTo("Test");
    }

    [TestMethod]
    public void MapIfExist_ToTypeActionNull()
    {
        Compte? source = null;

        Check.ThatCode(() => _mapper.MapIfExist<CompteDto>(source, () => throw new KrosoftTechniqueException("Test")))
             .Throws<KrosoftTechniqueException>()
             .WithMessage("Test");
    }

    [TestMethod]
    public void MapIfExist_ToTypeNull()
    {
        Compte? source = null;

        var destination = _mapper.MapIfExist<CompteDto>(source);

        Check.That(destination).IsNull();
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _mapper = serviceProvider.GetRequiredService<IMapper>();
    }
}