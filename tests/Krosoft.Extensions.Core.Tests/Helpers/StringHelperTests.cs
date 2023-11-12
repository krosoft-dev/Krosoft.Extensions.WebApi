using System.Globalization;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class StringHelperTests : BaseTest
{
    [TestMethod]
    public void ToBase64Test()
    {
        var base64 = StringHelper.ToBase64("jwtToken");
        Check.That(base64).IsEqualTo("and0VG9rZW4=");
    }

    [TestMethod]
    public void ToBase64EmptyTest()
    {
        var base64 = StringHelper.ToBase64(string.Empty);
        Check.That(base64).IsEqualTo(string.Empty);
    }

    [TestMethod]
    public void ToBase64NullTest()
    {
        Check.ThatCode(() => StringHelper.ToBase64(null))
             .Throws<KrosoftTechniqueException>()
             .WithMessage("La variable 'payload' n'est pas renseignée.");
    }

    [TestMethod]
    public void GetAbbreviationNullTest()
    {
        Check.That(StringHelper.GetAbbreviation(null)).IsEqualTo(string.Empty);
    }

    [TestMethod]
    public void GetAbbreviationEmptyTest()
    {
        Check.That(StringHelper.GetAbbreviation(string.Empty)).IsEqualTo(string.Empty);
    }

    [TestMethod]
    [DataRow("T", "T")]
    [DataRow("T ", "T")]
    [DataRow(" T", "T")]
    [DataRow(" T ", "T")]
    [DataRow("au", "AU")]
    [DataRow("AU", "AU")]
    [DataRow("Au", "AU")]
    [DataRow("Thibault de Montaigu", "TD")]
    [DataRow("thibault De montaigu", "TD")]
    [DataRow("Thibault de Montaigu de Lyon en Lausane", "TD")]
    [DataRow("Admin", "AD")]
    [DataRow("Administrator", "AD")]
    [DataRow("administrator", "AD")]
    [DataRow("AdministratorAuService", "AA")]
    [DataRow("Pierre Louis-Calixte de la Comédie-Française", "PL")]
    [DataRow("Claire de La Rüe du Can de la Comédie-Française", "CD")]
    public void GetAbbreviationTest(string input, string expected)
    {
        var formatDate = StringHelper.GetAbbreviation(input);
        Check.That(formatDate).IsEqualTo(expected);
    }

    [TestMethod]
    [DataRow("test", "01/01/0001")]
    [DataRow(null!, "01/01/0001")]
    [DataRow("", "01/01/0001")]
    [DataRow("12/08/1988", "12/08/1988")]
    [DataRow("2012-04-23T18:25:43.511Z", "23/04/2012")]
    public void FormatDateStringIncorrectTest(string input, string expected)
    { 
        var formatDate = StringHelper.FormatDate(input);
        Check.That(formatDate).IsEqualTo(expected);
    }

    [TestMethod]
    [DataRow(null, "")]
    [DataRow("", "")]
    [DataRow("   test   ", "test")]
    [DataRow("test   ", "test")]
    [DataRow("     test   ", "test")]
    public void TrimTest(string input, string expected)
    {
        var formatDate = StringHelper.Trim(input);
        Check.That(formatDate).IsEqualTo(expected);
    }

    [TestMethod]
    [DataRow(null, 0)]
    [DataRow("", 0)]
    [DataRow("160519", 160519)]
    [DataRow("9432.0", 0)]
    [DataRow("16,667", 0)]
    [DataRow("42.42", 0)]
    [DataRow("   -322   ", -322)]
    [DataRow("+4302", 4302)]
    [DataRow("(100);", 0)]
    [DataRow("01FA", 0)]
    public void TryParseToIntTest(string input, int expected)
    {
        var formatDate = StringHelper.TryParseToInt(input);
        Check.That(formatDate).IsEqualTo(expected);
    }
}