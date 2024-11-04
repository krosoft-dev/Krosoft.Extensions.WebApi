using Krosoft.Extensions.Cqrs.Models.Queries;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Queries;

public class CacheQuery : AuthBaseQuery<IDictionary<string, long>>;