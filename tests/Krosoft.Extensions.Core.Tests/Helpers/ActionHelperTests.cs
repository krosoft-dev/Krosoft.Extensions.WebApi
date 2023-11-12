using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Samples.Library.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class ActionHelperTests
{
    [TestMethod]
    public async Task ApplyWithAsyncTest()
    {
        var cities = AddresseFactory.GetAdresses()
                                    .Select(c => c.Ville);

        var adresses = await ActionHelper.ApplyWithAsync(cities, CompteFactory.ToCompteAsync);

        Check.That(adresses).HasSize(5);
        Check.That(adresses.Select(x => x.Name)).ContainsExactly("city3", "city4", "city", "city1", "city2");
    }
}