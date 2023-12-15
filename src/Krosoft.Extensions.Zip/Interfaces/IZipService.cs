using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Zip.Interfaces;

public interface IZipService
{
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

    Stream Zip(IDictionary<string, Stream> streams);

    Stream Zip(Stream stream, string fileName);

    Stream GetZipStream(IEnumerable<string> filePaths);

    void ExtractZip(string zipPath, string extractPath);
}