using MediatR;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Events;

public record GlobalRefreshCacheEvent : INotification;