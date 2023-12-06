using Krosoft.Extensions.Cqrs.Models.Queries;
using Krosoft.Extensions.Samples.DotNet8.Api.Models.Dto;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Models.Queries;

public class LogicielDetailQuery : AuthBaseQuery<LogicielDetailDto>
{
    public LogicielDetailQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}