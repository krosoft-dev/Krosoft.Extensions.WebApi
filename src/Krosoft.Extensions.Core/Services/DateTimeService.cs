using Krosoft.Extensions.Core.Interfaces;

namespace Krosoft.Extensions.Core.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}