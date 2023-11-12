using System.ComponentModel;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extension pour les énumérations.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Retourne la description à afficher d'une valeur d'énumération, soit depuis l'attribut
    /// <see cref="DescriptionAttribute" />,
    /// soit depuis la valeur si l'attribut n'est pas présent.
    /// </summary>
    /// <param name="value">Valeur d'énumération pour laquelle retourner le nom.</param>
    /// <returns>Nom à afficher de la valeur d'énumération.</returns>
    public static string GetDescription(this Enum? value)
    {
        Guard.IsNotNull(nameof(value), value);

        var type = value!.GetType();
        var fieldInfo = type.GetField(value.ToString());
        if (fieldInfo != null)
        {
            var attributes = (DescriptionAttribute[])fieldInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
        }

        return value.ToString();
    }

    /// <summary>
    /// Retourne le nom à afficher d'une valeur d'énumération, soit depuis l'attribut <see cref="DisplayNameAttribute" />,
    /// soit depuis la valeur si l'attribut n'est pas présent.
    /// </summary>
    /// <param name="value">Valeur d'énumération pour laquelle retourner le nom.</param>
    /// <returns>Nom à afficher de la valeur d'énumération.</returns>
    public static string GetDisplayName(this Enum? value)
    {
        Guard.IsNotNull(nameof(value), value);

        var type = value!.GetType();
        var fieldInfo = type
            .GetField(value.ToString());
        if (fieldInfo != null)
        {
            var attributes = (DisplayNameAttribute[])fieldInfo
                .GetCustomAttributes(typeof(DisplayNameAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].DisplayName;
            }
        }

        return value.ToString();
    }

    public static IEnumerable<Enum> GetFlags(this Enum? value)
    {
        if (value != null)
        {
            return GetFlags(value, Enum.GetValues(value.GetType()).Cast<Enum>().ToArray());
        }

        return new List<Enum>();
    }

    public static IEnumerable<Enum> GetIndividualFlags(this Enum? value)
    {
        if (value != null)
        {
            return GetFlags(value, GetFlagValues(value.GetType()).ToArray());
        }

        return new List<Enum>();
    }

    //checks to see if an enumerated value contains a type
    public static bool Has<T>(this Enum? type, T value)
    {
        try
        {
            return ((int?)(object?)type & (int?)(object?)value) == (int?)(object?)value;
        }
        catch
        {
            return false;
        }
    }

    private static IEnumerable<Enum> GetFlags(Enum? value, Enum[] values)
    {
        var bits = Convert.ToUInt64(value);
        var results = new List<Enum>();
        for (var i = values.Length - 1; i >= 0; i--)
        {
            var mask = Convert.ToUInt64(values[i]);
            if (i == 0 && mask == 0L)
            {
                break;
            }

            if ((bits & mask) == mask)
            {
                results.Add(values[i]);
                bits -= mask;
            }
        }

        if (bits != 0L)
        {
            return Enumerable.Empty<Enum>();
        }

        if (Convert.ToUInt64(value) != 0L)
        {
            return results.Reverse<Enum>();
        }

        if (bits == Convert.ToUInt64(value) && values.Length > 0 && Convert.ToUInt64(values[0]) == 0L)
        {
            return values.Take(1);
        }

        return Enumerable.Empty<Enum>();
    }

    private static IEnumerable<Enum> GetFlagValues(Type enumType)
    {
        ulong flag = 0x1;
        foreach (var value in Enum.GetValues(enumType).Cast<Enum>())
        {
            var bits = Convert.ToUInt64(value);
            if (bits == 0L)
                //yield return value;
            {
                continue; // skip the zero value
            }

            while (flag < bits)
            {
                flag <<= 1;
            }

            if (flag == bits)
            {
                yield return value;
            }
        }
    }
}