using System.Text;
using CsvHelper;
using Krosoft.Extensions.Reporting.Csv.Helpers;
using Krosoft.Extensions.Reporting.Csv.Interfaces;

namespace Krosoft.Extensions.Reporting.Csv.Services;

public class CsvReadService : ICsvReadService
{
    public IEnumerable<T> GetRecordsFromBase64<T>(IEnumerable<string> files, Encoding encoding, string cultureInfo)
    {
        var allRecords = new List<T>();
        foreach (var file in files)
        {
            var records = GetRecordsFromBase64<T>(file, encoding, cultureInfo);
            allRecords.AddRange(records);
        }

        return allRecords;
    }

    public IEnumerable<T> GetRecordsFromBase64<T>(string file, Encoding encoding, string cultureInfo)
    {
        var s = file.Substring(file.IndexOf(',') + 1);
        var fromBase64String = Convert.FromBase64String(s);
        using (var memoryStream = new MemoryStream(fromBase64String))
        using (var streamReader = new StreamReader(memoryStream, encoding))
        {
            var config = CsvConfigurationHelper.GetCsvConfiguration(cultureInfo);

            var csv = new CsvReader(streamReader, config);
            return csv.GetRecords<T>().ToList();
        }
    }

    public IEnumerable<T> GetRecordsFromPath<T>(string csvPath, Encoding encoding, string cultureInfo)
    {
        using (var streamReader = new StreamReader(csvPath, encoding))
        {
            var config = CsvConfigurationHelper.GetCsvConfiguration(cultureInfo);
            var csv = new CsvReader(streamReader, config);
            return csv.GetRecords<T>().ToList();
        }
    }
}