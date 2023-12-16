using Krosoft.Extensions.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class Base64HelperTests
{
    [DataTestMethod]
    [DataRow("SGVsbG8sIFdvcmxkIQ==", "Hello, World!")]
    [DataRow("", "")]
    [DataRow(null, null)]
    public void Base64ToString_ShouldConvertBase64ToString(string base64EncodedData, string expectedPlainText)
    {
        // Act
        var result = Base64Helper.Base64ToString(base64EncodedData);

        // Assert
        Check.That(result).IsEqualTo(expectedPlainText);
    }
}