using System.Collections;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using Krosoft.Extensions.Core.Helpers;
using NFluent;
using NFluent.Extensibility;
using NFluent.Kernel;

namespace Krosoft.Extensions.Testing.Extensions;

public static class NFluentExtension
{
    public static IEnumerable Extracting<T>(this IEnumerable<T> enumerable, Expression<Func<T, object>> expression) => enumerable.Extracting(PropertyNameHelper.For(expression));

    public static ICheckLink<ICheck<decimal>> IsEqualWithDelta(this ICheck<decimal> check, decimal target, decimal delta)
    {
        var runnableCheck = ExtensibilityHelper.ExtractChecker(check);

        var value = runnableCheck.Value;
        return runnableCheck.ExecuteCheck(() =>
                                          {
                                              if (Math.Abs(value - target) > delta)
                                              {
                                                  throw new FluentCheckException($"La valeur {value} n'est pas égale à {target} avec un delta de {delta}");
                                              }
                                          }, $"La valeur {value} est égale à {target} avec un delta de {delta}");
    }

    public static IEnumerable Extracting<T>(this DataRowCollection rows, Expression<Func<T, object>> expression)
    {
        foreach (DataRow dtRow in rows)
        {
            var value = dtRow[PropertyNameHelper.For(expression)].ToString();
            yield return value;
        }
    }

    public static IEnumerable Extracting<T>(this DataTable dt,
                                            Expression<Func<T, object>> expression)
        => dt.Rows.Extracting(expression);

    public static ICheck<string> IsFileEqualToEmbeddedFile(this ICheck<string> checkFilePath, Assembly executingAssembly, string resourceName, int numberOfLine)
    {
        var runnableCheckFilePath = ExtensibilityHelper.ExtractChecker(checkFilePath);

        var filePath = runnableCheckFilePath.Value;
        var executeCheck = runnableCheckFilePath.ExecuteCheck(() =>
        {
            Check.That(filePath).IsNotNull();

            var source = FileHelper.ReadAsStringArray(filePath).ToArray();
            var cible = FileHelper.ReadAsStringArray(executingAssembly, resourceName).ToArray();

            Check.That(source).HasSize(numberOfLine);
            Check.That(cible).HasSize(numberOfLine);

            for (var line = 0; line < source.Length; line++)
            {
                Check.That(source[line]).IsEqualTo(cible[line]);
            }
        }, string.Empty);

        return executeCheck.And;
    }
}