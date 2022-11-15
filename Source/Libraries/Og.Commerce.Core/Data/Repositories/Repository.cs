using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Og.Commerce.Core.Data.Extensions;
using Og.Commerce.Core.Domain;
using System.Linq.Expressions;

namespace Og.Commerce.Core.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    #region [ Fields ]

    private readonly DbContext _dbContext;
    #endregion

    #region [ Ctor ]

    public Repository(DbContext dbContext)
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

    public virtual async Task<bool> AnyAsync(bool includeDeleted = false, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return await Table.AnyAsync(cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Table.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<int> CountAsync(bool includeDeleted = false, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return await Table.CountAsync(cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Table.CountAsync(predicate, cancellationToken);
    }

    public virtual async Task<long> LongCountAsync(bool includeDeleted = false, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return await Table.LongCountAsync(cancellationToken);
    }

    public virtual async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Table.LongCountAsync(predicate, cancellationToken);
    }

    #endregion

    #region [ Transaction Methods ]

    public virtual async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var savedEntity = (await GetDbSet().AddAsync(entity, cancellationToken)).Entity;

        if (autoSave)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return savedEntity;
    }

    public virtual async Task InsertManyAsync(IList<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        await GetDbSet().AddRangeAsync(entities, cancellationToken);

        if (autoSave)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        _dbContext.Attach(entity);

        var updatedEntity = _dbContext.Update(entity).Entity;

        if (autoSave)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return updatedEntity;
    }

    public virtual async Task UpdateManyAsync(IList<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        GetDbSet().UpdateRange(entities);

        if (autoSave)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        GetDbSet().Remove(entity);

        if (autoSave)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task DeleteManyAsync(IList<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        GetDbSet().RemoveRange(entities);

        if (autoSave)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task<bool> DeleteByIdAsync<TKey>(TKey id, bool autoSave = false, CancellationToken cancellationToken = default) where TKey : notnull
    {
        var entity = await GetAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return false;
        }

        await DeleteAsync(entity, autoSave, cancellationToken);
        return true;
    }

    public virtual async Task DeleteByIdsAsync<TKey>(IList<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default) where TKey : notnull
    {
        var entities = await GetByIdsAsync(ids, cancellationToken: cancellationToken);

        await DeleteManyAsync(entities, autoSave, cancellationToken);
    }

    #endregion

    #region [ Helper Methods ]

    public DbSet<TEntity> GetDbSet() => _dbContext.Set<TEntity>();

    #endregion
}
