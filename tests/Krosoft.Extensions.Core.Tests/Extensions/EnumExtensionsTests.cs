using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.Library.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class EnumExtensionsTests
{
    [DataTestMethod]
    [DataRow(SampleCode.One, "Description for Value1")]
    [DataRow(SampleCode.Two, "Description for Value2")]
    [DataRow(SampleCode.Three, "Description for Value3")]
    [DataRow(SampleCode.Four, "Description for Value4")]
    [DataRow(SampleCode.Five, "Five")]
    public void GetDescription_ShouldReturnCorrectDescription(SampleCode value, string expectedDescription)
    {
        // Act
        var result = value.GetDescription();

        // Assert
        Check.That(result).IsEqualTo(expectedDescription);
    }

    [DataTestMethod]
    [DataRow(SampleCode.One, "Display Name for Value1")]
    [DataRow(SampleCode.Two, "Display Name for Value2")]
    [DataRow(SampleCode.Three, "Display Name for Value3")]
    [DataRow(SampleCode.Four, "Display Name for Value4")]
    [DataRow(SampleCode.Five, "Five")]
    public void GetDisplayName_ShouldReturnCorrectDisplayName(SampleCode value, string expectedDisplayName)
    {
        // Act
        var result = value.GetDisplayName();

        // Assert
        Check.That(result).IsEqualTo(expectedDisplayName);
    }

    [TestMethod]
    public void GetFlags_ShouldReturnCorrectFlags()
    {
        // Arrange
        var value = SampleCode.One | SampleCode.Three;

        // Act
        var result = value.GetFlags();

        // Assert
        Check.That(result).ContainsExactly(SampleCode.One, SampleCode.Three);
    }

    [TestMethod]
    public void GetIndividualFlags_ShouldReturnCorrectIndividualFlags()
    {
        // Arrange
        var value = SampleCode.One | SampleCode.Three;

        // Act
        var result = value.GetIndividualFlags();

        // Assert
        Check.That(result).ContainsExactly(SampleCode.One, SampleCode.Three);
    }
}