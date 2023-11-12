using System.Globalization;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Converters;

public class NullableIntegerConverter : JsonConverter<int?>
{
    public override void WriteJson(JsonWriter writer, int? value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }

    public override int? ReadJson(JsonReader reader, Type objectType, int? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value != null)
        {
            var input = reader.Value.ToString();
            if (input != null)
            {
                var value = new string(input.Where(char.IsDigit).ToArray());
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return int.Parse(value, CultureInfo.InvariantCulture);
                }
            }
        }

        return null;
    }
}