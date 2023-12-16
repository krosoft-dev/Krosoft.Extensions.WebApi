using System.Text;
using System.Text.RegularExpressions;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Helpers;

/// <summary>
/// Méthodes d'aide pour les objets de type <see cref="string" />.
/// </summary>
public static class StringHelper
{
    private static readonly Random Random = new Random();

    public static string ClearFilePath(string text) =>
        text
            .Replace(" ", "-")
            .Replace("/", "-");

    public static string FormatCurrency(decimal montant, string currencyIsoCode) => $"{currencyIsoCode} {FormatNumber(montant)}";

    public static string FormatDate(string dateString)
    {
        DateTime.TryParse(dateString, out var date);
        return date.ToString("d");
    }

    public static string FormatNumber(decimal montant) => $"{montant:# ##0.00}".Replace('.', ',');

    public static Stream GenerateStreamFromString(string? s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public static string GetAbbreviation(string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return string.Empty;
        }

        var trimedData = data.Trim();
        if (trimedData.Length <= 2)
        {
            return trimedData.ToUpper();
        }

        if (trimedData.Contains(' '))
        {
            var splited = trimedData.Split(' ');
            return $"{splited[0].ToUpper()[0]}{splited[1].ToUpper()[0]}";
        }

        var pascalCase = trimedData;
        pascalCase = trimedData.ToUpper()[0] + pascalCase.Substring(1);
        var upperCaseOnly = string.Concat(pascalCase.Where(char.IsUpper));
        if (upperCaseOnly.Length > 1 && upperCaseOnly.Length <= 3)
        {
            return upperCaseOnly.Substring(0, 2).ToUpper();
        }

        if (trimedData.Length <= 3)
        {
            return trimedData.Substring(0, 2).ToUpper();
        }

        return trimedData.Substring(0, 2).ToUpper();
    }

    /// <summary>Concatène plusieurs chaines et enlève les espaces superflus ou renvoie null si la chaine est vide</summary>
    /// <param name="chaines">Les chaines a formater</param>
    /// <returns>La chaine formatée </returns>
    public static string? Join(params object?[] chaines)
    {
        var chainesTemp = chaines.ToList().Where(s => s != null).Select(s => s!.ToString()!.Trim()).ToList();

        if (!chainesTemp.Any())
        {
            return string.Empty;
        }

        var chaine = string.Join(" ", chainesTemp);

        if (string.IsNullOrWhiteSpace(chaine))
        {
            return null;
        }

        return chaine.Trim();
    }

    public static string KeepDigitsOnly(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return new string(value.Where(char.IsDigit).ToArray());
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
                                    .Select(s => s[Random.Next(s.Length)])
                                    .ToArray());
    }

    public static string ToBase64(string? payload)
    {
        Guard.IsNotNull(nameof(payload), payload);

        var bytes = Encoding.GetEncoding(28591).GetBytes(payload!);
        var dataBase64 = Convert.ToBase64String(bytes);
        return dataBase64;
    }

    /// <summary>
    /// Supprime, de l'objet System.String actuel, tous les espaces blancs en début ou en fin de chaîne.
    /// </summary>
    /// <param name="value">la chaine de caractère</param>
    /// <param name="allWhiteSpace">
    /// Si allWhiteSpace est vrai, on supprime aussi tous les espaces blancs à l'intérieur de la
    /// chaine de caractères
    /// </param>
    /// <returns>une chaine de caractères sans espaces blancs</returns>
    public static string Trim(string value, bool allWhiteSpace = false)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (allWhiteSpace)
        {
            return Regex.Replace(value, @"\s+", string.Empty, RegexOptions.None, RegexHelper.MatchTimeout);
        }

        return value.Trim();
    }

    /// <summary>Enleve les espaces superflus d'une chaine uniquement ou renvoie null si la chaine est vide</summary>
    /// <param name="chaine">La chaine a formater</param>
    /// <returns>La chaine formatée </returns>
    public static string? Trim(object? chaine)
    {
        if (chaine == null)
        {
            return null;
        }

        var value = chaine.ToString();
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }

    /// <summary>
    /// Enleve les espaces superflus d'une chaine uniquement ou renvoie string.Empty si la chaine est vide.
    /// </summary>
    /// <param name="chaine">La chaine à formater.</param>
    /// <returns>La chaine formatée.</returns>
    public static string TrimIfNotNull(object? chaine)
    {
        if (chaine == null)
        {
            return string.Empty;
        }

        var value = chaine.ToString();
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return value.Trim();
    }

    public static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return value.Length <= maxLength
            ? value
            : value.Substring(0, maxLength) + "...";
    }

    public static bool TryParseToBoolean(string value)
    {
        var result = bool.TryParse(value, out var boolValue);
        if (result)
        {
            return boolValue;
        }

        return false;
    }

    /// <summary>
    /// Essaye de convertir en int, si echec on retourne 0
    /// </summary>
    /// <param name="value">la chaine de caractère à convertir</param>
    /// <returns>la chaine convertie, zero si echec</returns>
    public static int TryParseToInt(string? value)
    {
        var result = int.TryParse(value, out var number);
        if (result)
        {
            return number;
        }

        return 0;
    }

    /// <summary>
    /// Essaye de convertir en int, si echec on retourne 0
    /// </summary>
    /// <param name="value">la chaine de caractère à convertir</param>
    /// <returns>la chaine convertie, zero si echec</returns>
    public static long TryParseToLong(string value)
    {
        var result = long.TryParse(value, out var number);
        if (result)
        {
            return number;
        }

        return 0;
    }
}