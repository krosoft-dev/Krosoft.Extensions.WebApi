using System.Text;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Krosoft.Extensions.WebApi.Tests.Extensions;

[TestClass]
public class FileStreamExtensionsTests
{
    [TestMethod]
    public void ToFileStreamResult_GivenValidFileStream_ReturnsFileStreamResult()
    {
        var content = "Test content";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var fileStream = new GenericFileStream(stream, "test.txt", "text/plain");

        var result = fileStream.ToFileStreamResult();

        Check.That(result).IsNotNull();
        Check.That(result.FileDownloadName).IsEqualTo("test.txt");
        Check.That(result.ContentType).IsEqualTo("text/plain");

        Check.That(result.FileStream.To<string>()).IsEqualTo(content);
    }

    [TestMethod]
    public async Task ToFileStreamResult_Generic_GivenValidTask_ReturnsFileStreamResult()
    {
        var content = "Test content async";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var fileStreamTask = Task.FromResult<IFileStream>(new GenericFileStream(stream, "test_async.txt", "text/plain"));

        var result = await fileStreamTask.ToFileStreamResult();

        Check.That(result).IsNotNull();
        Check.That(result.FileDownloadName).IsEqualTo("test_async.txt");
        Check.That(result.ContentType).IsEqualTo("text/plain");

        Check.That(result.FileStream.To<string>()).IsEqualTo(content);
    }

    [TestMethod]
    public void ToFileResult_GivenValidFileStream_ReturnsFileStreamResult()
    {
        var content = "Test content";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var fileStream = new GenericFileStream(stream, "test.txt", "text/plain");

        var result = fileStream.ToFileResult() as FileStreamHttpResult;

        Check.That(result).IsNotNull();
        Check.That(result!.FileDownloadName).IsEqualTo("test.txt");
        Check.That(result.ContentType).IsEqualTo("text/plain");
        Check.That(result.FileStream.To<string>()).IsEqualTo(content);
    }

    [TestMethod]
    public async Task ToFileResult_Generic_GivenValidTask_ReturnsFileStreamResult()
    {
        var content = "Test content async";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var fileStreamTask = Task.FromResult<IFileStream>(new GenericFileStream(stream, "test_async.txt", "text/plain"));

        var result = await fileStreamTask.ToFileResult() as FileStreamHttpResult;

        Check.That(result).IsNotNull();
        Check.That(result!.FileDownloadName).IsEqualTo("test_async.txt");
        Check.That(result.ContentType).IsEqualTo("text/plain");
        Check.That(result.FileStream.To<string>()).IsEqualTo(content);
    }
}