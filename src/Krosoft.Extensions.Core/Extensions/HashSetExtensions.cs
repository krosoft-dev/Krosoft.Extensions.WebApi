namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extensions pour les classes implémentant l'interface <see cref="HashSet{T}" />.
/// </summary>
public static class HashSetExtensions
{
    public static void AddRange<T>(this HashSet<T> list, IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            list.Add(item);
        }
    }
}