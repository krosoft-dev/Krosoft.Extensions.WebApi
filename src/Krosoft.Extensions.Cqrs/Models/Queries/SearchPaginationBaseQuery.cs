using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Cqrs.Models.Queries;

public abstract class SearchPaginationBaseQuery<T> : PaginationBaseQuery<T>, ISearchPaginationRequest
{
    public string? Text { get; set; }
}