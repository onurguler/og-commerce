using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Og.Commerce.Core.Data.Extensions;

public static class DataExtensions
{
    /// <summary>
    /// Gets primary key properties of given entity
    /// </summary>
    /// <typeparam name="T">Type of the entity.</typeparam>
    /// <param name="dbContext">DbContext</param>
    /// <returns>
    /// List of primary keys
    /// </returns>
    public static IList<IProperty> GetPrimaryKeyProperties<T>(this DbContext dbContext)
    {
        return dbContext?.Model?.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties.ToList() ?? new List<IProperty>();
    }

    /// <summary>
    /// Returns a expression that contains given id parameters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dbContext"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> FilterByPrimaryKeyPredicate<T>(this DbContext dbContext, params object[] id)
    {
        var keyProperties = dbContext.GetPrimaryKeyProperties<T>();
        var parameter = Expression.Parameter(typeof(T), "e");
        var body = keyProperties
            // e => e.PK[i] == id[i]
            .Select((p, i) => Expression.Equal(
                Expression.Property(parameter, p.Name),
                Expression.Convert(
                    Expression.PropertyOrField(Expression.Constant(new { id = id[i] }), "id"),
                    p.ClrType)))
            .Aggregate(Expression.AndAlso);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    /// <summary>
    /// Filter db set by given primary key values
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="dbSet"></param>
    /// <param name="context"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static IQueryable<TEntity> FilterByPrimaryKey<TEntity>(this DbSet<TEntity> dbSet, DbContext context, params object[] id)
        where TEntity : class
    {
        return dbSet.Where(context.FilterByPrimaryKeyPredicate<TEntity>(id));
    }
}
