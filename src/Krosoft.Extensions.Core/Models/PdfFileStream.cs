using System.Net.Mime;

namespace Krosoft.Extensions.Core.Models;

public class PdfFileStream : GenericFileStream
{
    public PdfFileStream(Stream stream,
                         string fileName)
        : base(stream, fileName, MediaTypeNames.Application.Pdf)
    {
    }
}