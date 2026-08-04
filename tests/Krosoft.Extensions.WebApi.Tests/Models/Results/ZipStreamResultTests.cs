using System.IO.Compression;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.WebApi.Models.Results;
using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Tests.Models.Results;

[TestClass]
public class ZipStreamResultTests
{
    private DirectoryInfo _directory = null!;
    private string _firstFilePath = null!;
    private string _secondFilePath = null!;

    [TestInitialize]
    public void Initialize()
    {
        _directory = Directory.CreateTempSubdirectory("zip-stream-result");
        _firstFilePath = Path.Combine(_directory.FullName, "file1.bin");
        _secondFilePath = Path.Combine(_directory.FullName, "file2.bin");
        File.WriteAllBytes(_firstFilePath, Enumerable.Range(0, 2048).Select(i => (byte)(i % 256)).ToArray());
        File.WriteAllBytes(_secondFilePath, Enumerable.Range(0, 1024).Select(i => (byte)(i % 256)).ToArray());
    }

    [TestCleanup]
    public void Cleanup() => _directory.Delete(true);

    [TestMethod]
    public void Constructor_GivenNullDictionary_Throws()
        => Check.ThatCode(() => new ZipStreamResult(null!, "archive.zip"))
                .Throws<KrosoftTechnicalException>();

    [TestMethod]
    public void Constructor_GivenEmptyFileName_Throws()
        => Check.ThatCode(() => new ZipStreamResult(new Dictionary<string, string>(), " "))
                .Throws<KrosoftTechnicalException>();

    [TestMethod]
    public async Task ExecuteAsync_GivenFiles_WritesZipInResponseBody()
    {
        var filePathsByEntryName = new Dictionary<string, string>
        {
            { "file1.bin", _firstFilePath },
            { "folder/file2.bin", _secondFilePath }
        };

        var body = new MemoryStream();
        var httpContext = CreateHttpContext(body);

        await new ZipStreamResult(filePathsByEntryName, "archive.zip").ExecuteAsync(httpContext);

        Check.That(httpContext.Response.ContentType).IsEqualTo("application/zip");
        Check.That(httpContext.Response.Headers.ContentDisposition.ToString())
             .IsEqualTo("attachment; filename=archive.zip; filename*=UTF-8''archive.zip");

        var bytes = body.ToArray();
        Check.That(bytes.Take(4)).ContainsExactly((byte)'P', (byte)'K', (byte)3, (byte)4);

        var entries = ReadEntries(body);
        Check.That(entries.Keys).ContainsExactly("file1.bin", "folder/file2.bin");
        Check.That(entries.Values).ContainsExactly(2048L, 1024L);
    }

    [TestMethod]
    public async Task ExecuteAsync_GivenWindowsSeparator_WritesZipCompliantEntryName()
    {
        var filePathsByEntryName = new Dictionary<string, string> { { @"folder\file2.bin", _secondFilePath } };

        var body = new MemoryStream();

        await new ZipStreamResult(filePathsByEntryName, "archive.zip").ExecuteAsync(CreateHttpContext(body));

        var entries = ReadEntries(body);
        Check.That(entries.Keys).ContainsExactly("folder/file2.bin");
    }

    [TestMethod]
    public async Task ExecuteAsync_GivenMissingFile_SkipsEntry()
    {
        var filePathsByEntryName = new Dictionary<string, string>
        {
            { "file1.bin", _firstFilePath },
            { "introuvable.bin", Path.Combine(_directory.FullName, "introuvable.bin") }
        };

        var body = new MemoryStream();

        await new ZipStreamResult(filePathsByEntryName, "archive.zip").ExecuteAsync(CreateHttpContext(body));

        var entries = ReadEntries(body);
        Check.That(entries.Keys).ContainsExactly("file1.bin");
    }

    [TestMethod]
    public async Task ExecuteAsync_GivenNoCompression_KeepsFileSize()
    {
        var filePathsByEntryName = new Dictionary<string, string> { { "file1.bin", _firstFilePath } };

        var body = new MemoryStream();

        await new ZipStreamResult(filePathsByEntryName, "archive.zip", CompressionLevel.NoCompression)
            .ExecuteAsync(CreateHttpContext(body));

        body.Position = 0;
        using var archive = new ZipArchive(body, ZipArchiveMode.Read);
        var entry = archive.Entries.Single();
        Check.That(entry.Length).IsEqualTo(2048);
        Check.That(entry.CompressedLength).IsEqualTo(2048);
    }

    [TestMethod]
    public async Task ExecuteAsync_GivenEmptyDictionary_WritesEmptyZip()
    {
        var body = new MemoryStream();

        await new ZipStreamResult(new Dictionary<string, string>(), "archive.zip").ExecuteAsync(CreateHttpContext(body));

        Check.That(ReadEntries(body)).IsEmpty();
    }

    private static Dictionary<string, long> ReadEntries(MemoryStream body)
    {
        body.Position = 0;
        using var archive = new ZipArchive(body, ZipArchiveMode.Read, true);
        return archive.Entries.ToDictionary(e => e.FullName, e => e.Length);
    }

    private static DefaultHttpContext CreateHttpContext(Stream body)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = body;
        return httpContext;
    }
}
