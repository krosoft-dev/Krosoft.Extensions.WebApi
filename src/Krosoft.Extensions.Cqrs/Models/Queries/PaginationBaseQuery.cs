using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Cqrs.Models.Queries;

public abstract record PaginationBaseQuery<T> : BaseQuery<PaginationResult<T>>, IPaginationRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public ISet<string> SortBy { get; set; } = new HashSet<string>();
}