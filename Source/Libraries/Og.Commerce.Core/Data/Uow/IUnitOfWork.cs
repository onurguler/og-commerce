using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Og.Commerce.Core.Data.Repositories;

namespace Og.Commerce.Core.Data.Uow;

/// <summary>
/// Defines the interfaces for <see cref="IRepository{TEntity,TPrimaryKey}"/> interfaces.
/// </summary>
public interface IRepositoryFactory
{
    /// <summary>
    /// Gets the specified repository for the <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="hasCustomRepository"><c>True</c> if providing custom repository</param>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
    IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false)
        where TEntity : class;
}

public interface IUnitOfWork : IRepositoryFactory
{
    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    int SaveChanges();

    /// <summary>
    /// Asynchronously saves all changes made in this unit of work to the database.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Executes the specified raw SQL command.
    /// </summary>
    /// <param name="sql">The raw SQL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The number of state entities written to database.</returns>
    int ExecuteSqlCommand(string sql, params object[] parameters);

    /// <summary>
    /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity"/> data.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="sql">The raw SQL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>An <see cref="IQueryable{T}"/> that contains elements that satisfy the condition specified by raw SQL.</returns>
    IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;

    /// <summary>
    /// Uses TrakGrap Api to attach disconnected entities
    /// </summary>
    /// <param name="rootEntity"> Root entity</param>
    /// <param name="callback">Delegate to convert Object's State properities to Entities entry state.</param>
    void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback);
}

/// <summary>
/// Defines the interface(s) for generic unit of work.
/// </summary>
public interface IUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    /// <summary>
    /// Gets the db context.
    /// </summary>
    /// <returns>The instance of type <typeparamref name="TDbContext"/>.</returns>
    TDbContext DbContext { get; }

    /// <summary>
    /// Saves all changes made in this context to the database with distributed transaction.
    /// </summary>
    /// <param name="unitOfWorks">An optional <see cref="IUnitOfWork"/> array.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
    Task<int> SaveChangesAsync(params IUnitOfWork[] unitOfWorks);
}