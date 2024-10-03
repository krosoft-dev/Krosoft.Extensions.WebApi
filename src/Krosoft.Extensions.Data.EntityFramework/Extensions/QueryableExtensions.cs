using AutoMapper;
using AutoMapper.QueryableExtensions;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Data.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginationResult<TOuput>> ToPaginationAsync<TEntity, TOuput>(this IQueryable<TEntity> query,
                                                                                          IPaginationRequest request,
                                                                                          IConfigurationProvider configurationProvider,
                                                                                          CancellationToken cancellationToken)
    {
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
                          .ProjectTo<TOuput>(configurationProvider)
                          .SortBy(request)
                          .Skip((request.PageNumber - 1) * request.PageSize)
                          .Take(request.PageSize)
                          .ToListAsync(cancellationToken);

        return new PaginationResult<TOuput>(items, totalCount, request.PageNumber, request.PageSize);
    }

    public static async Task<PaginationResult<T>> ToPaginationAsync<T>(this IQueryable<T> query,
                                                                       IPaginationRequest request,
                                                                       CancellationToken cancellationToken)
    {
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
                          .Skip((request.PageNumber - 1) * request.PageSize)
                          .Take(request.PageSize)
                          .ToListAsync(cancellationToken);

        return new PaginationResult<T>(items, totalCount, request.PageNumber, request.PageSize);
    }
}