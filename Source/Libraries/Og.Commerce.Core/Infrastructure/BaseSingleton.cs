namespace Og.Commerce.Core.Infrastructure;

/// <summary>
/// Provides access to all "singletons" stored by <see cref="Singleton{T}"/>.
/// </summary>
public class BaseSingleton
{
    /// <summary>
    /// Ctor
    /// </summary>
    static BaseSingleton()
    {
        AllSingletons = new Dictionary<Type, object?>();
    }

    /// <summary>
    /// Dictionary of type to singleton instances.
    /// </summary>
    public static IDictionary<Type, object?> AllSingletons { get; }
}
