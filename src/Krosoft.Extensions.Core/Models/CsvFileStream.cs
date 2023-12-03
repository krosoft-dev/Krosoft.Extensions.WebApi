namespace Krosoft.Extensions.Core.Models;

public class CsvFileStream : GenericFileStream
{
    public CsvFileStream(Stream stream,
                         string fileName)
        : base(stream, fileName, "text/csv")
    {
    }
}