using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Cqrs.Models.Queries;

public abstract class AuthSearchPaginationBaseQuery<T> : AuthPaginationBaseQuery<T>, ISearchPaginationRequest
{
    public string? Text { get; set; }
}