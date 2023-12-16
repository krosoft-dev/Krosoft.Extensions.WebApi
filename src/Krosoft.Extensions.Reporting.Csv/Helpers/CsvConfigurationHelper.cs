using System.Globalization;
using System.Text;
using CsvHelper.Configuration;

namespace Krosoft.Extensions.Reporting.Csv.Helpers;

public static class CsvConfigurationHelper
{
    private static readonly List<string> CulturesOverridable = new List<string>
    {
        "fr",
        "fr-fr"
    };

    public static CsvConfiguration GetCsvConfiguration(CultureInfo cultureInfo)
    {
        if (CulturesOverridable.Contains(cultureInfo.Name.ToLowerInvariant()))
        {
            cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
        }

        var config = new CsvConfiguration(cultureInfo)
        {
            Delimiter = ";",
            MissingFieldFound = _ => { },
            BadDataFound = _ => { },
            Encoding = Encoding.UTF8
        };
        return config;
    }
}