namespace Krosoft.Extensions.Hosting.Models;

public record ScheduleConfig
{
    public TimeSpan Interval { get; set; }
}