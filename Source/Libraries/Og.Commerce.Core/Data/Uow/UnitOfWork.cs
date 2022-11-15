using Microsoft.EntityFrameworkCore;
using Og.Commerce.Core.Data.Repositories;

namespace Og.Commerce.Core.Data.Uow;

public sealed class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
{
    #region [ Fields ]

    private bool _disposed;
    private Dictionary<Type, object>? _repositories;

    private readonly TDbContext _dbContext;


    #endregion

    #region [ Ctor ]

    public UnitOfWork(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    #region [ Properties ]
    public TDbContext DbContext => _dbContext;

    #endregion

    #region [ Methods ]

    /// <summary>
    /// Gets the specified repository for the <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        _repositories ??= new Dictionary<Type, object>();

        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new Repository<TEntity>(_dbContext);
        }

        return (IRepository<TEntity>)_repositories[type];
    }

    /// <summary>
    /// Asynchronously saves all changes made in this unit of work to the database.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _dbContext.SaveChangesAsync(cancellationToken);


    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">The disposing.</param>
    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _repositories?.Clear();
                _dbContext.Dispose();
            }
        }
        _disposed = true;
    }

    #endregion
}
