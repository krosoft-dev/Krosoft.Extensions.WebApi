using Krosoft.Extensions.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class StringExtensionsTests
{
    [DataTestMethod]
    [DataRow("", 5, "")]
    [DataRow(null, 5, "")]
    [DataRow("", 5, "")]
    [DataRow("abcdefgh", 3, "abc")]
    [DataRow("xyz", 10, "xyz")]
    [DataRow("12345", 0, "")]
    [DataRow("text", -1, "")]
    public void Left_Test(string? input, int length, string? expectedOutput)
    {
        var result = input.Left(length);

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, null, false)]
    [DataRow(null, "aaa", false)]
    [DataRow("aaa", null, false)]
    [DataRow("", "", false)]
    [DataRow("aaa", "", false)]
    [DataRow("", "aaa", false)]
    [DataRow("text", "un text court", true)]
    [DataRow("text", "un TEXT court", true)]
    [DataRow("text", "unTEXTcourt", true)]
    [DataRow("TEXT", "unTEXTcourt", true)]
    [DataRow("TEXT", "un text court", true)]
    [DataRow("un TEXT", "unTEXTcourt", true)]
    [DataRow("un TEXT", "un-TEXT-court", true)]
    public void Match_Tests(string? searchText, string? text, bool expectedOutput)
    {
        var result = searchText.Match(text);
        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("", "")]
    [DataRow("abc 123", "abc123")]
    [DataRow("   leading and trailing spaces   ", "leadingandtrailingspaces")]
    [DataRow("nochange", "nochange")]
    public void RemoveAllSpaces_Tests(string? input, string? expectedOutput)
    {
        var result = input.RemoveAllSpaces();
        Check.That(result).IsEqualTo(expectedOutput);
    }

    [TestMethod]
    public void RemoveDiacriticsTest()
    {
        Check.That("včľťšľžšžščýščýťčáčáčťáčáťýčťž".RemoveDiacritics()).IsEqualTo("vcltslzszscyscytcacactacatyctz");
        Check.That("Rez-de-chaussée".RemoveDiacritics()).IsEqualTo("Rez-de-chaussee");
    }

    [DataTestMethod]
    [DataRow(null, "")]
    [DataRow("", "")]
    [DataRow("text", "text")]
    [DataRow("ét€", "t")]
    [DataRow("abc123", "abc123")]
    [DataRow("special!@#$characters", "specialcharacters")]
    [DataRow("remove spaces", "removespaces")]
    [DataRow("   leading and trailing spaces   ", "leadingandtrailingspaces")]
    public void RemoveSpecials_Tests(string input, string expectedOutput)
    {
        var result = input.RemoveSpecials();

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [TestMethod]
    public void Replace_Ok()
    {
        char[] separators = { ';', '.', ',' };
        var input = "this;is,a.test".Replace(separators, " ");
        Check.That(input).IsEqualTo("this is a test");
    }

    [DataTestMethod]
    [DataRow(null, "find", "replace", "")]
    [DataRow("", "find", "replace", "")]
    [DataRow("find and replace", "find", "new", "new and replace")]
    [DataRow("no match", "find", "replace", "no match")]
    [DataRow("replace first find and then find", "find", "Hello", "replace first Hello and then find")]
    public void ReplaceFirstOccurrence_Tests(string text, string seach, string replace, string expectedOutput)
    {
        var result = text.ReplaceFirstOccurrence(seach, replace);

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, "find", "replace", "")]
    [DataRow("", "find", "replace", "")]
    [DataRow("find and replace", "find", "new", "new and replace")]
    [DataRow("no match", "find", "replace", "no match")]
    [DataRow("replace first find and then find", "find", "Hello", "replace first find and then Hello")]
    public void ReplaceLastOccurrence_Tests(string text, string seach, string replace, string expectedOutput)
    {
        var result = text.ReplaceLastOccurrence(seach, replace);

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, 5, "")]
    [DataRow("", 5, "")]
    [DataRow("abcdefgh", 3, "fgh")]
    [DataRow("xyz", 10, "xyz")]
    [DataRow("12345", 0, "")]
    [DataRow("text", -1, "")]
    public void Right_Tests(string input, int length, string expectedOutput)
    {
        var result = input.Right(length);

        Check.That(result).IsEqualTo(expectedOutput);
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

    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("", "")]
    [DataRow("abc", "Abc")]
    [DataRow("aBC", "Abc")]
    [DataRow("hello world", "Hello world")]
    [DataRow("   leading space", "   leading space")] // Should not change leading space
    [DataRow("123", "123")]
    public void ToUpperFirst_Tests(string? input, string? expectedOutput)
    {
        var result = input.ToUpperFirst();

        Check.That(result).IsEqualTo(expectedOutput);
    }
}