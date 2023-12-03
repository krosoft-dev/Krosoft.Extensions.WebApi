using System.Net.Mime;

namespace Krosoft.Extensions.Core.Models;

public class ZipFileStream : GenericFileStream
{
    public ZipFileStream(Stream stream,
                         string fileName)
        : base(stream, fileName, MediaTypeNames.Application.Zip)
    {
    }
}