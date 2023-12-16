using System.Text;

namespace Krosoft.Extensions.Core.Helpers;

public static class Base64Helper
{
    /// <summary>
    /// Transforme un texte en base 64 en string
    /// </summary>
    /// <param name="base64EncodedData">texte en base 64</param>
    /// <returns>le texte en string</returns>
    public static string? Base64ToString(string? base64EncodedData)
    {
        if (base64EncodedData != null)
        {
            var encodedDataAsBytes = Convert.FromBase64String(base64EncodedData);
            var plainText = Encoding.UTF8.GetString(encodedDataAsBytes, 0, encodedDataAsBytes.Length);
            return plainText;
        }

        return null;
    }

    public static string? StringToBase64(string? plainText)
    {
        if (plainText != null)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        return null;
    }
}