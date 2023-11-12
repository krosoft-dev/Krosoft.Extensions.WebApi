using Krosoft.Extensions.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class CollectionExtensionsTests
{
    [TestMethod]
    public void AddRangeTest()
    {
        var codes = new HashSet<string>();
        codes.AddRange(new List<string> { "A", "B" });

        Check.That(codes).HasSize(2);
        Check.That(codes).ContainsExactly("A", "B");
    }
}