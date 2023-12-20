using System.Linq.Expressions;
using System.Reflection;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;
using LinqKit;

namespace Krosoft.Extensions.Data.Abstractions.Extensions;

public static class QueryableExtensions
{
    private static readonly MethodInfo? ContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
    private static readonly MethodInfo? ToLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

    public static IQueryable<T> Filter<T, TItem>(this IQueryable<T> query,
                                                 IEnumerable<TItem>? items,
                                                 Func<TItem, Expression<Func<T, bool>>>? func,
                                                 bool isMandatory = false)
    {
        Guard.IsNotNull(nameof(items), items);
        Guard.IsNotNull(nameof(func), func);

        var uniqueItems = items!.ToHashSet();
        if (!isMandatory && uniqueItems.Count <= 0)
        {
            return query;
        }

        var predicate = PredicateBuilder.New<T>();
        foreach (var item in uniqueItems)
        {
            predicate = predicate.Or(func!(item));
        }

        query = query.Where(predicate);

        return query;
    }

    public static IQueryable<T> Filter<T, TItem>(this IQueryable<T> query,
                                                 TItem? item,
                                                 Func<TItem?, Expression<Func<T, bool>>> getLambdaExpression)
        where TItem : struct
    {
        if (item.HasValue)
        {
            query = query.Where(getLambdaExpression(item));
        }

        return query;
    }

    public static IQueryable<T> Search<T>(this IQueryable<T> query,
                                          string? searchTerm,
                                          params Expression<Func<T, string?>>[] selectors)
    {
        if (ToLowerMethod == null)
        {
            throw new KrosoftTechniqueException("Method 'ToLower' is not defined.");
        }

        if (ContainsMethod == null)
        {
            throw new KrosoftTechniqueException("Method 'Contains' is not defined.");
        }

        if (string.IsNullOrEmpty(searchTerm))
        {
            return query;
        }

        Expression right = Expression.Constant(searchTerm);
        right = Expression.Call(right, ToLowerMethod);

        var predicate = PredicateBuilder.New<T>();
        foreach (var selector in selectors)
        {
            var left = selector.Body;
            left = Expression.Call(left, ToLowerMethod);
            var containsBound = Expression.Call(left, ContainsMethod, right);
            var lambda = Expression.Lambda<Func<T, bool>>(containsBound, selector.Parameters);

            predicate = predicate.Or(lambda);
        }

        return query.Where(predicate);
    }

    public static IQueryable<T> SearchAll<T>(this IQueryable<T> query,
                                             string? searchTerm,
                                             Expression<Func<T, string?>> selector)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return query;
        }

        var parts = searchTerm.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length > 0)
        {
            foreach (var part in parts)
            {
                query = query.Search(part, selector);
            }
        }

        return query;
    }
}