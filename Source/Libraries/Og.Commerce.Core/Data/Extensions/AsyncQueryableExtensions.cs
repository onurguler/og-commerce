using Microsoft.EntityFrameworkCore;
using Og.Commerce.Core.Domain;

namespace Og.Commerce.Core.Data.Extensions;

public static class AsyncQueryableExtensions
{
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="source">source</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T>? source, int pageIndex, int pageSize, bool getOnlyTotalCount = false, CancellationToken cancellationToken = default)
    {
        if (source == null)
            return new PagedList<T>(new List<T>(), pageIndex, pageSize);

        //min allowed page size is 1
        pageSize = Math.Max(pageSize, 1);

        var count = await source.CountAsync(cancellationToken);

        var data = new List<T>();

        if (!getOnlyTotalCount)
            data.AddRange(await source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync(cancellationToken));

        return new PagedList<T>(data, pageIndex, pageSize, count);
    }
}
