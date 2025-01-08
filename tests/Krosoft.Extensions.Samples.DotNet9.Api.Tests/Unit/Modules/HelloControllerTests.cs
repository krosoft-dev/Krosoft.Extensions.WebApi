using JetBrains.Annotations;
using Krosoft.Extensions.Samples.DotNet9.Api.Modules;
using Krosoft.Extensions.Samples.Library.Models.Queries;
using Krosoft.Extensions.Testing;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Tests.Unit.Modules;

[TestClass]
[TestSubject(typeof(HelloModule))]
public class HelloControllerTests : BaseTest
{
    [TestMethod]
    public async Task Hello_Ok()
    {
        var expectedOutput = "Hello-World";

        var mock = new Mock<IMediator>();
        mock.Setup(x => x.Send(It.IsAny<HelloDotNet9Query>(), CancellationToken.None))
            .ReturnsAsync(() => expectedOutput);

        var result = await HelloModule.Hello(mock.Object, CancellationToken.None);

        mock.Verify(x => x.Send(It.IsAny<HelloDotNet9Query>(), CancellationToken.None), Times.Once());

        Check.That(result).IsNotNull();
        Check.That(result).IsEqualTo(expectedOutput);
    }
}