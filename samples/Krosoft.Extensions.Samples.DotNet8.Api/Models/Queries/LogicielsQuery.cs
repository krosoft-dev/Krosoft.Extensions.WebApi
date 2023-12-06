using Krosoft.Extensions.Cqrs.Models.Queries;
using Krosoft.Extensions.Samples.DotNet8.Api.Models.Dto;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Models.Queries;

public class LogicielsQuery : BaseQuery<IEnumerable<LogicielDto>>
{
    public string? Nom { get; set; }
}