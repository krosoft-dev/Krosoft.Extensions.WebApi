using System.IO.Compression;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.Interfaces;
using Krosoft.Extensions.WebApi.Models;

namespace Krosoft.Extensions.WebApi.Tests.Extensions;

[TestClass]
public class ZipStreamExtensionsTests
{
    private static readonly Dictionary<string, string> FilePathsByEntryName = new() { { "file.bin", @"c:\temp\file.bin" } };

    [TestMethod]
    public void ToZipStreamResult_GivenZipFiles_ReturnsZipStreamResult()
    {
        var zipFiles = new ZipFiles("archive.zip", FilePathsByEntryName);

        var result = zipFiles.ToZipStreamResult();

        Check.That(result).IsNotNull();
        Check.That(result.FileName).IsEqualTo("archive.zip");
    }

    [TestMethod]
    public void ToZipStreamResult_GivenNullZipFiles_Throws()
        => Check.ThatCode(() => ((IZipFiles)null!).ToZipStreamResult())
                .Throws<KrosoftTechnicalException>();

    [TestMethod]
    public async Task ToZipStreamResult_GivenTask_ReturnsZipStreamResult()
    {
        var task = Task.FromResult(new ZipFiles("archive_async.zip", FilePathsByEntryName));

        var result = await task.ToZipStreamResult(CompressionLevel.NoCompression);

        Check.That(result).IsNotNull();
        Check.That(result.FileName).IsEqualTo("archive_async.zip");
    }
}
