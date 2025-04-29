using System.Text;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Krosoft.Extensions.WebApi.Tests.Extensions;

[TestClass]
public class HttpRequestExtensionsTests
{
    [TestMethod]
    public async Task ToFileAsync_Ok()
    {
        var expectedContent = "Test content";
        var expectedFileName = "test.txt";
        var contentBytes = Encoding.UTF8.GetBytes(expectedContent);

        var requestMock = new Mock<HttpRequest>();
        requestMock.Setup(r => r.Body).Returns(new MemoryStream(contentBytes));

        var result = await requestMock.Object.ToFileAsync(expectedFileName, CancellationToken.None);

        Check.That(result).IsNotNull();
        Check.That(result!.Name).IsEqualTo(expectedFileName);
        Check.That(result.Content).IsEqualTo(contentBytes);
    }

    [TestMethod]
    public async Task ToBase64StringAsync_Ok()
    {
        var fileContent = "Hello, world!";
        var fileBytes = Encoding.UTF8.GetBytes(fileContent);
        var formFile = new FormFile(new MemoryStream(fileBytes), 0, fileBytes.Length, "file", "hello.txt");

        var formCollection = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { formFile });

        var requestMock = new Mock<HttpRequest>();
        requestMock.Setup(r => r.ReadFormAsync(CancellationToken.None)).ReturnsAsync(formCollection);

        var base64Expected = Convert.ToBase64String(fileBytes);

        var result = await requestMock.Object.ToBase64StringAsync()!.ToList();

        Check.That(result).IsNotNull();
        Check.That(result).HasSize(1);
        Check.That(result[0]).IsEqualTo(base64Expected);
    }
}