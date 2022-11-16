using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Og.Commerce.Core.Data.Repositories;

namespace Og.Commerce.Core.Data.Uow;

/// <summary>
/// Extension methods for setting up unit of work related services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class UnitOfWorkServiceCollectionExtensions
{
    /// <summary>
    /// Registers the unit of work given context as a service in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TContext1">The type of the db context.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    /// <remarks>
    /// This method only support one db context, if been called more than once, will throw exception.
    /// </remarks>
    public static IServiceCollection AddUnitOfWork<TContext1>(this IServiceCollection services)
        where TContext1 : DbContext
    {
        services.AddScoped<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();

        return services;
    }


    /// <summary>
    /// Registers the custom repository as a service in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TRepository">The type of the custom repositry.</typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddCustomRepository<TEntity, TPrimaryKey, TRepository>(
        this IServiceCollection services)
        where TEntity : class
        where TRepository : class, IRepository<TEntity>
    {
        services.AddScoped<IRepository<TEntity>, TRepository>();

        return services;
    }
}
