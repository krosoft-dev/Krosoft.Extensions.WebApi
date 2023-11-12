using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.Library.Factories;
using Krosoft.Extensions.Samples.Library.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class DictionaryExtensionsTests
{
    [TestMethod]
    public void GetValueOrDefaultDictionaryTest()
    {
        Dictionary<string, Addresse> adressesParStreetLine1 = AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("street1Line1");
        Check.That(address).IsNotNull();
        Check.That(address!.Ligne2).IsEqualTo("street1Line2");
    }

    [TestMethod]
    public void GetValueOrDefaultIDictionaryTest()
    {
        IDictionary<string, Addresse> adressesParStreetLine1 = AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("street1Line1");
        Check.That(address).IsNotNull();
        Check.That(address!.Ligne2).IsEqualTo("street1Line2");
    }

    [TestMethod]
    public void GetValueOrDefaultDictionaryDefaultValueTest()
    {
        Dictionary<string, Addresse> adressesParStreetLine1 = AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("test");
        Check.That(address).IsNull();
    }

    [TestMethod]
    public void GetValueOrDefaultIDictionaryDefaultValueTest()
    {
        IDictionary<string, Addresse> adressesParStreetLine1 = AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("test");
        Check.That(address).IsNull();
    }
}