using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.Library.Factories;
using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class DictionaryExtensionsTests
{
    [TestMethod]
    public void GetValueOrDefaultDictionaryDefaultValueTest()
    {
        var adressesParStreetLine1 = AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("test");
        Check.That(address).IsNull();
    }

    [TestMethod]
    public void GetValueOrDefaultDictionaryTest()
    {
        var adressesParStreetLine1 = AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("street1Line1");
        Check.That(address).IsNotNull();
        Check.That(address!.Ligne2).IsEqualTo("street1Line2");
    }

    [TestMethod]
    public void GetValueOrDefaultIDictionaryDefaultValueTest()
    {
        IDictionary<string, Addresse> adressesParStreetLine1 = AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("test");
        Check.That(address).IsNull();
    }

    [TestMethod]
    public void GetValueOrDefaultIDictionaryTest()
    {
        IDictionary<string, Addresse> adressesParStreetLine1 = AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("street1Line1");
        Check.That(address).IsNotNull();
        Check.That(address!.Ligne2).IsEqualTo("street1Line2");
    }
}