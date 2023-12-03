using System.Text;

namespace Krosoft.Extensions.Reporting.Csv.Interfaces
{
    public interface ICsvReadService
    {
        IEnumerable<T> GetRecordsFromBase64<T>(IEnumerable<string> files, Encoding encoding, string cultureInfo);
        IEnumerable<T> GetRecordsFromBase64<T>(string file, Encoding encoding, string cultureInfo);
        IEnumerable<T> GetRecordsFromPath<T>(string csvPath, Encoding encoding, string cultureInfo);
    }
}