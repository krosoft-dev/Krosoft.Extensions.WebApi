using Krosoft.Extensions.Samples.Library.Models;
using Krosoft.Extensions.Testing.Extensions;
using Moq;

namespace Krosoft.Extensions.Testing.Tests.Extensions;

[TestClass]
public class MockExtensionTests
{
    [TestMethod]
    public void VerifyNeverCallTest()
    {
        var foo = new Mock<IFoo>(MockBehavior.Loose);

        Check.That(foo).Verify(m => m.Call1(), Times.Never);
        Check.That(foo).Verify(m => m.Call2(), Times.Never);
    }

    [TestMethod]
    public void VerifyTest()
    {
        var foo = new Mock<IFoo>(MockBehavior.Loose);

        foo.Object.Call1();

        Check.That(foo).Verify(m => m.Call1(), Times.Once);
        Check.That(foo).Verify(m => m.Call2(), Times.Never);
    }
}