using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Zip.Interfaces;

public interface IZipService
{
    void ExtractZip(string zipPath, string extractPath);

    Stream GetZipStream(IEnumerable<string> filePaths);

    Stream Zip(IDictionary<string, Stream> streams);

    Stream Zip(Stream stream, string fileName);

    Task<ZipFileStream> ZipAsync(IReadOnlyDictionary<string, string> dictionary,
                                 string fileName,
                                 CancellationToken cancellationToken);

    Task<Stream> ZipAsync(IReadOnlyDictionary<string, string> dictionary,
                          CancellationToken cancellationToken);

    Task<ZipFileStream> ZipAsync(IReadOnlyDictionary<string, Stream> dictionary,
                                 string fileName,
                                 CancellationToken cancellationToken);

    Task<Stream> ZipAsync(IReadOnlyDictionary<string, Stream> dictionary,
                          CancellationToken cancellationToken);
}