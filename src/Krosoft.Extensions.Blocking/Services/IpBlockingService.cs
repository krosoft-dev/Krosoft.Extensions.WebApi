using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.Blocking.Abstractions.Models.Enums;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Blocking.Services;

public class IpBlockingService : BlockingService, IIpBlockingService
{
    public IpBlockingService(IBlockingStorageProvider blockingStorageProvider,
                             ILogger<IpBlockingService> logger)
        : base(BlockType.Ip, blockingStorageProvider, logger)
    {
    }
}