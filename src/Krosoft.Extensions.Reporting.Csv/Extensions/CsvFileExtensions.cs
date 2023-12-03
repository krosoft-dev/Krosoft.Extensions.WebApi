using CsvHelper;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Reporting.Csv.Helpers;
using Krosoft.Extensions.Reporting.Csv.Models;

namespace Krosoft.Extensions.Reporting.Csv.Extensions;

public static class CsvFileExtensions
{
    public static MemoryStream ToMemoryStream<T>(this CsvFileData<T> csvFile)
    {
        var result = csvFile.ToBytes();
        var memoryStream = new MemoryStream(result);
        return memoryStream;
    }

    public static byte[] ToBytes<T>(this CsvFileData<T> csvFile)
    {
        var config = CsvConfigurationHelper.GetCsvConfiguration(csvFile.Culture);
        using (var memoryStream = new MemoryStream())
        using (var streamWriter = new StreamWriter(memoryStream, config.Encoding))
        using (var csvWriter = new CsvWriter(streamWriter, config))
        {
            csvWriter.WriteRecords(csvFile.Data);
            streamWriter.Flush();
            return memoryStream.ToArray();
        }
    }

    public static IFileStream ToCsvStreamResult<T>(this CsvFileData<T> csvStreamFile)
    {
        var memoryStream = csvStreamFile.ToMemoryStream();
        return new CsvFileStream(memoryStream, csvStreamFile.FileName);
    }

    public static async Task<IFileStream> ToCsvStreamResult<T>(this Task<CsvFileData<T>> task)
    {
        var file = await task;
        return file.ToCsvStreamResult();
    }
}