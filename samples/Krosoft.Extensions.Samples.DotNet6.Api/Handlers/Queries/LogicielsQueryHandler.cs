using AutoMapper;
using AutoMapper.QueryableExtensions;
using Krosoft.Extensions.Samples.Library.Factories;
using Krosoft.Extensions.Samples.Library.Models.Dto;
using Krosoft.Extensions.Samples.Library.Models.Queries;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Handlers.Queries;

public class LogicielsQueryHandler : IRequestHandler<LogicielsQuery, IEnumerable<LogicielDto>>
{
    private readonly ILogger<LogicielsQueryHandler> _logger;
    private readonly IMapper _mapper;

    public LogicielsQueryHandler(ILogger<LogicielsQueryHandler> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LogicielDto>> Handle(LogicielsQuery request,
                                                       CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération des logiciels...");

        await Task.Delay(2000, cancellationToken);

        var logiciels = LogicielFactory.GetRandom(10, null)
                                       .AsQueryable()
                                       .ProjectTo<LogicielDto>(_mapper.ConfigurationProvider)
                                       .ToList();

        return logiciels;
    }
}