using Microsoft.AspNetCore.WebUtilities;

namespace Krosoft.Extensions.Samples.DotNet8.BlazorApp.Models;

public readonly struct Urls
{
    internal struct Api
    {
        internal static string GetLogiciels(string text)
        {
            var param = new Dictionary<string, string?> { { "text", text } };

            return UrlHelper.GetUrl("/Logiciels", param);
        }
    }

    internal static class UrlHelper
    {
        public static string GetUrl(string baseUrl,
                                    IEnumerable<KeyValuePair<string, string?>>? queryString,
                                    UriKind uriKind = UriKind.Relative)

        {
            Uri uri;
            if (queryString == null)
            {
                uri = new Uri(baseUrl, uriKind);
            }

            else
            {
                var url = baseUrl;

                foreach (var keyValuePair in queryString)
                {
                    if (!string.IsNullOrEmpty(keyValuePair.Value))
                    {
                        url = QueryHelpers.AddQueryString(url, keyValuePair.Key, keyValuePair.Value);
                    }
                }

                uri = new Uri(url, uriKind);
            }

            return uri.ToString();
        }
    }
}