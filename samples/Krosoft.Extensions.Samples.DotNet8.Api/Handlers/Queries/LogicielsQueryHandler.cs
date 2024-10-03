using AutoMapper;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Samples.Library.Factories;
using Krosoft.Extensions.Samples.Library.Models.Dto;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Samples.Library.Models.Queries;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Handlers.Queries;

public class LogicielsQueryHandler : IRequestHandler<LogicielsQuery, PaginationResult<LogicielDto>>
{
    private readonly ILogger<LogicielsQueryHandler> _logger;
    private readonly IMapper _mapper;

    public LogicielsQueryHandler(ILogger<LogicielsQueryHandler> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<PaginationResult<LogicielDto>> Handle(LogicielsQuery request,
                                                            CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération des logiciels...");

        await Task.Delay(2000, cancellationToken);

        var result = await LogicielFactory.GetRandom(10, null)
                                          .AsQueryable()
                                          .ToPaginationAsync<Logiciel, LogicielDto>(request,
                                                                                    _mapper.ConfigurationProvider,
                                                                                    cancellationToken);

        return result;
    }
}