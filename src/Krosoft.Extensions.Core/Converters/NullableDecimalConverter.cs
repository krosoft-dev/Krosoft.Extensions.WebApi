using System.Globalization;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Converters;

public class NullableDecimalConverter : JsonConverter<decimal?>
{
    public override void WriteJson(JsonWriter writer, decimal? value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }

    public override decimal? ReadJson(JsonReader reader, Type objectType, decimal? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value != null)
        {
            var input = reader.Value.ToString();
            if (input != null)
            {
                var value = new string(input.Where(c => char.IsDigit(c) || c == ',' || c == '.').ToArray()).Trim();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                }
            }
        }

        return null;
    }
}