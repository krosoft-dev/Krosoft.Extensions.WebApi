using System.Xml;

namespace Krosoft.Extensions.Reporting.Csv.Extensions;

public static class XmlDocumentExtensions
{
    public static void ForceArrayByTagName(this XmlDocument doc, string tagName)
    {
        var elements = doc.GetElementsByTagName(tagName);

        foreach (var element in elements)
        {
            if (element is XmlElement el)
            {
                var jsonArray = doc.CreateAttribute("json", "Array", "http://james.newtonking.com/projects/json");
                jsonArray.Value = "true";
                el.SetAttributeNode(jsonArray);
            }
        }
    }
}