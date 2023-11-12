using Krosoft.Extensions.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class NumberHelperTest
{
    [TestMethod]
    public void ToIntegerTest()
    {
        Assert.AreEqual(0, NumberHelper.ToInteger(null));
        Assert.AreEqual(0, NumberHelper.ToInteger(string.Empty));
        Assert.AreEqual(160519, NumberHelper.ToInteger("160519"));
        Assert.AreEqual(9432, NumberHelper.ToInteger("9432.0"));
        Assert.AreEqual(16, NumberHelper.ToInteger("16,667"));
        Assert.AreEqual(42, NumberHelper.ToInteger("42.42"));
        Assert.AreEqual(322, NumberHelper.ToInteger("   -322   "));
        Assert.AreEqual(4302, NumberHelper.ToInteger("+4302"));
        Assert.AreEqual(100, NumberHelper.ToInteger("(100);"));
        Assert.AreEqual(1, NumberHelper.ToInteger("01FA"));
        Assert.AreEqual(100, NumberHelper.ToInteger("100 100"));
    }
}