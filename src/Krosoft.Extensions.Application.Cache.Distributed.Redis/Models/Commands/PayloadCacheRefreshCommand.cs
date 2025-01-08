using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Commands;

public record PayloadCacheRefreshCommand : PayloadBaseCommand
{
    public PayloadCacheRefreshCommand(string payload) : base(payload)
    {
    }
}