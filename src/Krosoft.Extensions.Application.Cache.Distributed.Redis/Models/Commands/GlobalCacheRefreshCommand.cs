using Krosoft.Extensions.Cqrs.Models.Commands;
using MediatR;

namespace Krosoft.Extensions.Application.Cache.Distributed.Redis.Models.Commands;

public class GlobalCacheRefreshCommand : BaseCommand<Unit>;