using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Zip.Interfaces;

public interface IZipService
{
    Task<ZipFileStream> ZipAsync(IReadOnlyDictionary<string, string> dictionary,
                                 string fileName,
                                 CancellationToken cancellationToken);

    Stream Zip(IDictionary<string, Stream> streams);

    Stream Zip(Stream stream, string fileName);

    Stream GetZipStream(IEnumerable<string> filePaths);

    void ExtractZip(string zipPath, string extractPath);
}