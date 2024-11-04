using Krosoft.Extensions.Hosting.Models;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Models;

public class CacheScheduleConfig<TCommand> : ScheduleConfig
{
    public TCommand? Command { get; set; }
}