using AutoMapper;
using AutoMapper.QueryableExtensions;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Samples.Library.Models.Dto;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Samples.Library.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Handlers.Queries;

public class LanguesQueryHandler : IRequestHandler<LanguesQuery, IEnumerable<LangueDto>>
{
    private readonly ILogger<LanguesQueryHandler> _logger;
    private readonly IReadRepository<Langue> _repository;
    private readonly IMapper _mapper;

    public LanguesQueryHandler(ILogger<LanguesQueryHandler> logger, IReadRepository<Langue> repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LangueDto>> Handle(LanguesQuery request,
                                                     CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération des langues...");

        var langues = await _repository.Query().ProjectTo<LangueDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        return langues;
    }
}