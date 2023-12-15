using System.Globalization;

namespace Krosoft.Extensions.Reporting.Csv.Models;

public struct CsvFileData<T>
{
    public CsvFileData(IEnumerable<T> data, string fileName, CultureInfo culture)
    {
        Data = data;
        FileName = fileName;
        Culture = culture;
    }

    public IEnumerable<T> Data { get; }
    public string FileName { get; }
    public CultureInfo Culture { get; }
}