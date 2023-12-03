using System.IO.Compression;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Zip.Interfaces;

namespace Krosoft.Extensions.Zip.Services;

/// <summary>
/// Service de gestion des fichiers ZIP.
/// </summary>
public class ZipService : IZipService
{
    public async Task<ZipFileStream> ZipAsync(IReadOnlyDictionary<string, string> dictionary,
                                              string fileName,
                                              CancellationToken cancellationToken)
    {
        Guard.IsNotNull(nameof(dictionary), dictionary);
        Guard.IsNotNullOrWhiteSpace(nameof(fileName), fileName);
        var ms = new MemoryStream();
        if (dictionary.Count > 0)
        {
            using var archive = new ZipArchive(ms, ZipArchiveMode.Create, true);
            foreach (var x in dictionary)
            {
                if (File.Exists(x.Value))
                {
                    var entry = archive.CreateEntry(x.Key);
                    await using var entryStream = entry.Open();
                    await using var fileStream = File.OpenRead(x.Value);
                    await fileStream.CopyToAsync(entryStream, cancellationToken);
                }
            }

            await ms.FlushAsync(cancellationToken);
            ms.Seek(0, SeekOrigin.Begin);
        }

        return new ZipFileStream(ms, fileName.Sanitize());
    }

    public Stream Zip(Stream stream, string fileName)
    {
        Guard.IsNotNull(nameof(stream), stream);
        Guard.IsNotNullOrWhiteSpace(nameof(fileName), fileName);

        var memoryStream = new MemoryStream();

        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            var entry = archive.CreateEntry(fileName.Sanitize());
            using (var entryStream = entry.Open())
            {
                stream.CopyTo(entryStream);
            }
        }

        memoryStream.Position = 0;
        return memoryStream;
    }

    public void ExtractZip(string zipPath, string extractPath)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(zipPath), zipPath);
        Guard.IsNotNullOrWhiteSpace(nameof(extractPath), extractPath);

        if (Directory.Exists(extractPath))
        {
            var di = new DirectoryInfo(extractPath);

            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }

            Directory.Delete(extractPath);
        }

        Directory.CreateDirectory(extractPath);
        ZipFile.ExtractToDirectory(zipPath, extractPath);
    }

    public Stream Zip(IDictionary<string, Stream> streams)
    {
        Guard.IsNotNull(nameof(streams), streams);

        var memoryStream = new MemoryStream();

        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var fileStream in streams)
            {
                var entry = archive.CreateEntry(fileStream.Key);
                using (var entryStream = entry.Open())
                {
                    fileStream.Value.CopyTo(entryStream);
                }
            }
        }

        memoryStream.Position = 0;
        return memoryStream;
    }

    public Stream GetZipStream(IEnumerable<string> filePaths)
    {
        Guard.IsNotNull(nameof(filePaths), filePaths);

        var filesStreams = new Dictionary<string, Stream>();
        foreach (var path in filePaths)
        {
            var fileStream = File.Open(path, FileMode.Open);
            filesStreams.Add(Path.GetFileName(path), fileStream);
        }

        var zipStream = Zip(filesStreams);

        foreach (var fileStream in filesStreams.Values)
        {
            fileStream.Dispose();
        }

        return zipStream;
    }
}