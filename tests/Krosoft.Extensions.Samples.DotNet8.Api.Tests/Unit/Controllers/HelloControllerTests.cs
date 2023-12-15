using Krosoft.Extensions.Samples.DotNet8.Api.Controllers;
using Krosoft.Extensions.Samples.Library.Models.Queries;
using Krosoft.Extensions.Testing.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Tests.Unit.Controllers;

[TestClass]
public class HelloControllerTests : ControllerBaseTest<HelloController>
{
    [TestMethod]
    public async Task HelloWorldAsync_Ok()
    {
        var expectedOutput = "Hello-World";

        Mock.Setup(x => x.Send(It.IsAny<HelloDotNet8Query>(), CancellationToken.None))
            .ReturnsAsync(() => expectedOutput);

        var result = await Controller.HelloWorldAsync(CancellationToken.None);

        Mock.Verify(x => x.Send(It.IsAny<HelloDotNet8Query>(), CancellationToken.None), Times.Once());

        Check.That(result).IsNotNull();
        Check.That(result).IsEqualTo(expectedOutput);
    }
}