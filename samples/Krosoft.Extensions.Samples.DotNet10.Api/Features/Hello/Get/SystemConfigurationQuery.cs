using Krosoft.Extensions.Cqrs.Models.Queries;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Hello.Get;

internal record SystemConfigurationQuery(Guid Id) : BaseQuery<string>;