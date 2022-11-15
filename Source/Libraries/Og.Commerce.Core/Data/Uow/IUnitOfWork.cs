using Og.Commerce.Core.Data.Repositories;

namespace Og.Commerce.Core.Data.Uow;

public interface IUnitOfWork<TDbContext> : IDisposable where TDbContext : class
{
    TDbContext DbContext { get; }
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
