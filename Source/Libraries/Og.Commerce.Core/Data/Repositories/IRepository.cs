using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Og.Commerce.Core.Domain;
using System.Linq.Expressions;

namespace Og.Commerce.Core.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Table { get; }

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TEntity?> GetAsync(object[] keyValues, CancellationToken cancellationToken = default);
        Task<TEntity?> GetAsync<TKey>(TKey id, CancellationToken cancellationToken = default) where TKey : notnull;
        Task<IList<TEntity>> GetByIdsAsync<TKey>(IList<TKey> ids, CancellationToken cancellationToken = default) where TKey : notnull;
        DbSet<TEntity> GetDbSet();
        Task<IList<TEntity>> GetListAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null, CancellationToken cancellationToken = default);
        Task<IList<TEntity>> GetListByAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includes = null, CancellationToken cancellationToken = default);
        Task<IPagedList<TEntity>> GetListPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, CancellationToken cancellationToken = default);
        Task<long> LongCountAsync(CancellationToken cancellationToken = default);
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        void Remove(TEntity entity);
        void RemoveRange(IList<TEntity> entities);
        TEntity Update(TEntity entity);
        void UpdateRange(IList<TEntity> entities);
    }
}