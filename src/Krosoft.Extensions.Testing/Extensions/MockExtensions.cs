using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using NFluent;
using NFluent.Extensibility;
using NFluent.Kernel;

namespace Krosoft.Extensions.Testing.Extensions;

public static class MockExtensions
{
    public static void Verify<T>(this Mock<ILogger<T>> mock, LogLevel level, string message, Times times)
    {
        mock.Verify(x => x.Log(level,
                               It.IsAny<EventId>(),
                               It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains(message)),
                               It.IsAny<Exception>(),
                               ((Func<It.IsAnyType, Exception, string>)It.IsAny<object>())!),
                    times);
    }

    public static ICheck<Mock<T>> Verify<T>(this ICheck<Mock<T>> check, Expression<Action<T>> expression, Func<Times> times, string? failMessage = null)
        where T : class
    {
        var runnableCheck = ExtensibilityHelper.ExtractChecker(check);

        var value = runnableCheck.Value;
        var executeCheck = runnableCheck.ExecuteCheck(() =>
        {
            try
            {
                value.Verify(expression, times, failMessage);
            }
            catch (MockException e)
            {
                throw new FluentCheckException(e.Message);
            }
        }, string.Empty);

        return executeCheck.And;
    }
}