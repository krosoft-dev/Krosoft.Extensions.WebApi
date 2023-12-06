using Krosoft.Extensions.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
    public void MatchTest()
    {
        Check.That("text".Match("un text court")).IsTrue();
        Check.That("text".Match("un TEXT court")).IsTrue();
        Check.That("text".Match("unTEXTcourt")).IsTrue();
        Check.That("TEXT".Match("unTEXTcourt")).IsTrue();
        Check.That("TEXT".Match("un text court")).IsTrue();
        Check.That("un TEXT".Match("unTEXTcourt")).IsTrue();
        Check.That("un TEXT".Match("un-TEXT-court")).IsTrue();
    }

    [TestMethod]
    public void RemoveSpecialsTest()
    {
        Check.That("text".RemoveSpecials()).IsEqualTo("text");
        Check.That("ét€".RemoveSpecials()).IsEqualTo("t");
    }

    [TestMethod]
    public void Sanitize1Test()
    {
        Check.That("asdf.txt".Sanitize()).IsEqualTo("asdf.txt");
    }

    [TestMethod]
    public void Sanitize2Test()
    {
        Check.That("\"<>|:*?\\/.txt".Sanitize()).IsEqualTo("_________.txt");
    }

    [TestMethod]
    public void Sanitize3Test()
    {
        Check.That("yes_its_valid_~!@#$%^&()_+.txt".Sanitize()).IsEqualTo("yes_its_valid__.txt");
    }

    [TestMethod]
    public void Sanitize4Test()
    {
        Check.That("*_*.txt".Sanitize("Yo")).IsEqualTo("Yo_Yo.txt");
    }

    [TestMethod]
    public void RemoveDiacriticsTest()
    {
        Check.That("včľťšľžšžščýščýťčáčáčťáčáťýčťž".RemoveDiacritics()).IsEqualTo("vcltslzszscyscytcacactacatyctz");
        Check.That("Rez-de-chaussée".RemoveDiacritics()).IsEqualTo("Rez-de-chaussee");
    }

    [TestMethod]
    public void Replace_Ok()
    {
        char[] separators = { ';', '.', ',' };
        var input = "this;is,a.test".Replace(separators, " ");
        Check.That(input).IsEqualTo("this is a test");
    }
}