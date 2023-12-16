using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extensions pour la classe <see cref="string" />.
/// </summary>
public static class StringExtensions
{
    private static readonly Regex RemoveInvalidChars = new Regex($"[{Regex.Escape(new string(GetInvalidFileNameChars()))}]",
                                                                 RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant
                                                                 , RegexHelper.MatchTimeout);

    public static string? ClearFilePath(this string? text) => StringHelper.ClearFilePath(text);

    public static string FromUtf8ToAscii(this string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        var utf8 = Encoding.UTF8;
        var encodedBytes = utf8.GetBytes(text);
        var targetEncoding = Encoding.GetEncoding(1252);
        var convertedBytes = Encoding.Convert(Encoding.UTF8, targetEncoding, encodedBytes);
        return targetEncoding.GetString(convertedBytes);
    }

    private static char[] GetInvalidFileNameChars() => new[]
    {
        '\"', '<', '>', '|', '\0',
        (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10,
        (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20,
        (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
        (char)31, ':', '*', '?', '\\', '/'
    };

    /// <summary>
    /// Renvoie les n caractères de gauche
    /// </summary>
    /// <param name="text">la chaine</param>
    /// <param name="length">le nombre de caractères</param>
    /// <returns>une sous chaine</returns>
    public static string Left(this string? text, int length)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        if (length < 0)
        {
            return string.Empty;
        }

        return text.Substring(0, Math.Min(length, text.Length));
    }

    public static bool Match(this string? searchText,
                             string? text)
    {
        if (string.IsNullOrEmpty(searchText) || string.IsNullOrEmpty(text))
        {
            return false;
        }

        return searchText.RemoveSpecials().Contains(text.RemoveSpecials(), StringComparison.InvariantCultureIgnoreCase) ||
               text.RemoveSpecials().Contains(searchText.RemoveSpecials(), StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Enlève tous les espaces d'une chaîne.
    /// </summary>
    /// <param name="text">Texte source.</param>
    /// <returns>Chaîne sans les espaces.</returns>
    public static string? RemoveAllSpaces(this string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return Regex.Replace(text, @"\s+", string.Empty, RegexOptions.None, RegexHelper.MatchTimeout);
    }

    /// <summary>
    /// Remplace tous les caractères avec accent par leurs version sans accent
    /// </summary>
    /// <param name="text">valeur à transformer</param>
    /// <returns>la valeur sans accent</returns>
    public static string RemoveDiacritics(this string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    public static string RemoveSpecials(this string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        return Regex.Replace(text, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled, RegexHelper.MatchTimeout);
    }

    public static string Replace(this string? text, char[] separators, string input)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        var temp = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(input, temp);
    }

    public static string ReplaceFirstOccurrence(this string? source, string find, string replace)
    {
        if (string.IsNullOrEmpty(source))
        {
            return string.Empty;
        }

        var place = source.IndexOf(find, StringComparison.Ordinal);
        if (place >= 0)
        {
            var result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        return source;
    }

    public static string ReplaceLastOccurrence(this string? source, string find, string replace)
    {
        if (string.IsNullOrEmpty(source))
        {
            return string.Empty;
        }

        var place = source.LastIndexOf(find, StringComparison.Ordinal);
        if (place >= 0)
        {
            var result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        return source;
    }

    /// <summary>
    /// Renvoie les n caractères de droite
    /// </summary>
    /// <param name="text">la chaine</param>
    /// <param name="length">le nombre de caractères</param>
    /// <returns>une sous chaine</returns>
    public static string Right(this string? text, int length)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        if (length < 0)
        {
            return string.Empty;
        }

        return text.Substring(Math.Max(0, text.Length - length), Math.Min(length, text.Length));
    }

    public static string Sanitize(this string? text,
                                  string replacement = "_")
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        return RemoveInvalidChars.Replace(text, replacement).RemoveSpecials();
    }

    /// <summary>
    /// Découpe une chaîne et nettoie les résultats.
    /// </summary>
    /// <param name="text">Chaîne à découper.</param>
    /// <param name="splitString">Chaîne de découpe.</param>
    /// <returns>Tableau des éléments trimmés après découpe, les éléments vides étant enlevés.</returns>
    public static string[] SplitAndClean(this string? text, char splitString)
    {
        if (string.IsNullOrEmpty(text))
        {
            return Array.Empty<string>();
        }

        var split = text.Split(splitString)
                        .Select(piece => new { piece, trimmed = piece.Trim() })
                        .Where(t => !string.IsNullOrEmpty(t.trimmed))
                        .Select(t => t.trimmed);

        return split.ToArray();
    }

    /// <summary>
    /// Extrait les caractères alpha-numériques de la valeur en paramètre.
    /// </summary>
    /// <param name="value">Valeur à extraire.</param>
    /// <returns>Valeur extraite.</returns>
    public static string ToAlphaNumeric(this string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var rgx = new Regex("[^a-zA-Z0-9]", RegexOptions.None, RegexHelper.MatchTimeout);
        return rgx.Replace(value, string.Empty);
    }

    public static string? ToCamelCase(this string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return char.ToLowerInvariant(value[0]) + value.Substring(1);
    }

    public static int ToInteger(this string? value) => NumberHelper.ToInteger(value);

    /// <summary>
    /// Convertie le premier caractère de la chaîne en majuscule, puis le reste en minuscule.
    /// </summary>
    /// <param name="text">Texte source.</param>
    /// <returns>Premier caractère de la chaîne en majuscule, puis le reste en minuscule.</returns>
    public static string? ToUpperFirst(this string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return char.ToUpper(text[0]) +
               (text.Length > 1
                   ? text.Substring(1).ToLower()
                   : string.Empty);
    }

    /// <summary>
    /// Tronque une chaine si sa longueur est plus grande que maxLenght
    /// Sinon, renvoie la même référence
    /// </summary>
    /// <param name="value">La chaine à tronquer</param>
    /// <param name="length">longueur maximale</param>
    /// <returns>La chaine tronquée</returns>
    public static string? Truncate(this string? value, int length)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        if (length < 0 || value.Length <= length)
        {
            return value;
        }

        return value.Substring(0, length);
    }
}