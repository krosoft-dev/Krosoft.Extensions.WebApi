using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class JsonHelperTests
{
    [TestMethod]
    public void GetTest()
    {
        var items = JsonHelper.Get<Item>(typeof(JsonHelperTests).Assembly).ToList();
        Check.That(items).IsNotNull();
        Check.That(items).HasSize(3);
        Check.That(items.Select(g => g.Libelle)).ContainsExactly("Item 001", "item 002 dolor consectetur", "item trois");
    }

    [TestMethod]
    public void IsValidTest()
    {
        var json = AssemblyHelper.ReadAsString(typeof(JsonHelperTests).Assembly, $"{nameof(Item)}.json", EncodingHelper.GetEuropeOccidentale());

        Check.That(JsonHelper.IsValid(json)).IsEqualTo(true);
    }

    [TestMethod]
    public void ToBase64NoObjectTest()
    {
        var base64 = JsonHelper.ToBase64(1);
        Check.That(base64).IsEqualTo("MQ==");
    }

    [TestMethod]
    public void ToBase64NullTest()
    {
        Check.ThatCode(() => JsonHelper.ToBase64(null))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'obj' n'est pas renseignée.");
    }

    [TestMethod]
    public void ToBase64Test()
    {
        var data = new
        {
            id = "requestId",
            jwt = "jwtToken"
        };

        var base64 = JsonHelper.ToBase64(data);

        Check.That(base64).IsEqualTo("eyJpZCI6InJlcXVlc3RJZCIsImp3dCI6Imp3dFRva2VuIn0=");
    }
}