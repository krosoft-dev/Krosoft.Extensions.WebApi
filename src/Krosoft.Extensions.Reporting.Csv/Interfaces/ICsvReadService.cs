using System.Globalization;
using System.Text;

namespace Krosoft.Extensions.Reporting.Csv.Interfaces
{
    public interface ICsvReadService
    {
        IEnumerable<T> GetRecordsFromBase64<T>(IEnumerable<string> files, Encoding encoding, CultureInfo cultureInfo);
        IEnumerable<T> GetRecordsFromBase64<T>(string file, Encoding encoding, CultureInfo cultureInfo);
        IEnumerable<T> GetRecordsFromPath<T>(string csvPath, Encoding encoding, CultureInfo cultureInfo);
    }
}