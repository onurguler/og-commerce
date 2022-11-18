using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Og.Commerce.Core.Data.Extensions;
using Og.Commerce.Core.Domain;
using System.Linq.Expressions;

namespace Og.Commerce.Core.Data.Repositories;

public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    #region [ Fields ]

    private readonly DbContext _dbContext;
    #endregion

    #region [ Ctor ]

    public EFRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    #region [ Properties ]

    public IQueryable<TEntity> Table => _dbContext.Set<TEntity>().AsNoTrackingWithIdentityResolution();

    #endregion

    #region [ Query Methods ]

    public virtual async Task<TEntity?> GetAsync<TKey>(TKey id, CancellationToken cancellationToken = default) where TKey : notnull
    {
        var pkExpression = _dbContext.FilterByPrimaryKeyPredicate<TEntity>(id);
        return await Table.FirstOrDefaultAsync(pkExpression, cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(object[] keyValues, CancellationToken cancellationToken = default)
    {
        var pkExpression = _dbContext.FilterByPrimaryKeyPredicate<TEntity>(keyValues);
        return await Table.FirstOrDefaultAsync(pkExpression, cancellationToken);
    }

    public virtual async Task<IList<TEntity>> GetByIdsAsync<TKey>(IList<TKey> ids, CancellationToken cancellationToken = default) where TKey : notnull
    {
        var keyProperties = _dbContext.GetPrimaryKeyProperties<TEntity>();
        var pkName = keyProperties?.FirstOrDefault()?.Name;
        if (string.IsNullOrWhiteSpace(pkName))
            throw new KeyNotFoundException($"Primary key not not found of entity '{nameof(TEntity)}'.");
        var entries = await Table.Where(w => ids.Contains(EF.Property<TKey>(w, pkName))).ToListAsync(cancellationToken).ConfigureAwait(false);

        var sortedEntries = new List<TEntity>();
        foreach (var id in ids)
        {
            var sortedEntry = entries.FirstOrDefault(_dbContext.FilterByPrimaryKeyPredicate<TEntity>(id).Compile());
            if (sortedEntry != null)
                sortedEntries.Add(sortedEntry);
        }

        return sortedEntries;
    }

    public virtual async Task<IList<TEntity>> GetListByAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includes = null, CancellationToken cancellationToken = default)
    {
        var query = Table.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (includes != null)
        {
            query = includes(query);
        }

        var items = await query.ToListAsync(cancellationToken).ConfigureAwait(false);

        return items;
    }

    public virtual async Task<IList<TEntity>> GetListAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null, CancellationToken cancellationToken = default)
    {
        var query = Table.AsQueryable();

        query = func != null ? await func(query) : query;

        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<IPagedList<TEntity>> GetListPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, CancellationToken cancellationToken = default)
    {
        var query = Table;

        query = func != null ? await func(query) : query;

        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount, cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await Table.AnyAsync(cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Table.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await Table.CountAsync(cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Table.CountAsync(predicate, cancellationToken);
    }

    public virtual async Task<long> LongCountAsync(CancellationToken cancellationToken = default)
    {
        return await Table.LongCountAsync(cancellationToken);
    }

    public virtual async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Table.LongCountAsync(predicate, cancellationToken);
    }

    #endregion

    #region [ Transaction Methods ]

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var savedEntity = (await GetDbSet().AddAsync(entity, cancellationToken)).Entity;
        return savedEntity;
    }

    public virtual async Task AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await GetDbSet().AddRangeAsync(entities, cancellationToken);
    }

    public virtual TEntity Update(TEntity entity)
    {
        _dbContext.Attach(entity);

        var updatedEntity = _dbContext.Update(entity).Entity;

        return updatedEntity;
    }

    public virtual void UpdateRange(IList<TEntity> entities)
    {
        GetDbSet().UpdateRange(entities);
    }

    public virtual void Remove(TEntity entity)
    {
        GetDbSet().Remove(entity);
    }

    public virtual void RemoveRange(IList<TEntity> entities)
    {
        GetDbSet().RemoveRange(entities);
    }

    #endregion

    #region [ Helper Methods ]

    public DbSet<TEntity> GetDbSet() => _dbContext.Set<TEntity>();

    #endregion
}
