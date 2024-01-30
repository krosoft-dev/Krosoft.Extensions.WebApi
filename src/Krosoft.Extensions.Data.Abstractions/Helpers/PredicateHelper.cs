using System.Linq.Expressions;
using Krosoft.Extensions.Core.Tools;
using LinqKit;

namespace Krosoft.Extensions.Data.Abstractions.Helpers;

public static class PredicateHelper
{
    public static ExpressionStarter<T> Or<T, TItem>(IEnumerable<TItem> items,
                                                    Func<TItem, Expression<Func<T, bool>>> func)
    {
        Guard.IsNotNull(nameof(items), items);
        Guard.IsNotNull(nameof(func), func);

        var predicate = PredicateBuilder.New<T>();
        foreach (var item in items)
        {
            predicate = predicate.Or(func(item));
        }

        return predicate;
    }
}