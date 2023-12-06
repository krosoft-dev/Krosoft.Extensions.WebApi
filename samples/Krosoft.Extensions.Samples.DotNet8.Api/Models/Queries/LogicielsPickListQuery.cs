using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Cqrs.Models.Queries;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Models.Queries;

public class LogicielsPickListQuery : AuthBaseQuery<IEnumerable<PickListDto>>
{
}