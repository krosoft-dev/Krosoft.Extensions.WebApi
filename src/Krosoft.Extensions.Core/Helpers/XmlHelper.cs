using System.Xml.Serialization;

namespace Krosoft.Extensions.Core.Helpers;

public static class XmlHelper
{
    public static T? Deserialize<T>(string? xml)
    {
        if (xml != null)
        {
            using (var reader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T?)serializer.Deserialize(reader);
            }
        }

        return default;
    }

    public static T? Deserialize<T>(Stream? reader)
    {
        var serializer = new XmlSerializer(typeof(T));
        if (reader != null)
        {
            return (T?)serializer.Deserialize(reader);
        }

        return default;
    }
}