using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Data.EntityFramework.Helpers;

public static class LoggerFactoryHelper
{
    public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddFilter((category, level) =>
                              category == DbLoggerCategory.Database.Command.Name &&
                              level == LogLevel.Information)
               .AddConsole();
    });
}