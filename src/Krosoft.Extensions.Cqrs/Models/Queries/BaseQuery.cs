namespace Krosoft.Extensions.Cqrs.Models.Queries;

public record BaseQuery<TResponse> : IQuery<TResponse>;