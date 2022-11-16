﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Og.Commerce.Core.Data.Repositories;
using System.Transactions;

namespace Og.Commerce.Core.Data.Uow;

/// <summary>
/// Represents the default implementation of the <see cref="IUnitOfWork"/> and <see cref="IUnitOfWork{TContext}"/> interface.
/// </summary>
/// <typeparam name="TContext">The type of the db context.</typeparam>
public sealed class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    private readonly TContext _context;
    private bool _disposed;
    private Dictionary<Type, object>? _repositories;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public UnitOfWork(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Gets the db context.
    /// </summary>
    /// <returns>The instance of type <typeparamref name="TContext"/>.</returns>
    public TContext DbContext => _context;

    /// <summary>
    /// Gets the specified repository for the <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="hasCustomRepository"><c>True</c> if providing custom repository</param>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
    public IRepository<TEntity> GetRepository<TEntity>(
        bool hasCustomRepository = false)
        where TEntity : class
    {
        _repositories ??= new Dictionary<Type, object>();

        // what's the best way to support custom repository?
        if (hasCustomRepository)
        {
            var customRepo = _context.GetService<IRepository<TEntity>>();
            return customRepo;
        }

        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new Repository<TEntity>(_context);
        }

        return (IRepository<TEntity>)_repositories[type];
    }

    /// <summary>
    /// Executes the specified raw SQL command.
    /// </summary>
    /// <param name="sql">The raw SQL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The number of state entities written to database.</returns>
    public int ExecuteSqlCommand(string sql, params object[] parameters) =>
        _context.Database.ExecuteSqlRaw(sql, parameters);

    /// <summary>
    /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity" /> data.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="sql">The raw SQL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>An <see cref="IQueryable{T}" /> that contains elements that satisfy the condition specified by raw SQL.</returns>
    public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class =>
        _context.Set<TEntity>().FromSqlRaw(sql, parameters);

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    /// <summary>
    /// Asynchronously saves all changes made in this unit of work to the database.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Saves all changes made in this context to the database with distributed transaction.
    /// </summary>
    /// <param name="unitOfWorks">An optional <see cref="IUnitOfWork"/> array.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
    public async Task<int> SaveChangesAsync(params IUnitOfWork[] unitOfWorks)
    {
        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var count = 0;
        foreach (var unitOfWork in unitOfWorks)
        {
            count += await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        count += await SaveChangesAsync();

        ts.Complete();

        return count;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);

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
                // clear _repositories
                if (_repositories != null)
                {
                    _repositories.Clear();
                }

                // dispose the db context.
                _context.Dispose();
            }
        }

        _disposed = true;
    }

    public void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback)
    {
        _context.ChangeTracker.TrackGraph(rootEntity, callback);
    }
}