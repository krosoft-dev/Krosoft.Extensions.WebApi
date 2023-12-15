using Krosoft.Extensions.Cqrs.Models.Queries;
using Krosoft.Extensions.Samples.Library.Models.Dto;

namespace Krosoft.Extensions.Samples.Library.Models.Queries;

public class LogicielsQuery : BaseQuery<IEnumerable<LogicielDto>>
{
    public string? Nom { get; set; }
}