using Microsoft.EntityFrameworkCore;
using Og.Commerce.Core.Data.Uow;
using Og.Commerce.Core.Infrastructure;

namespace Og.Commerce.Core.Application;

public abstract class ApplicationService<TDbContext> where TDbContext : DbContext
{
    private IUnitOfWork<TDbContext>? _unitOfWork;
    public IUnitOfWork<TDbContext> UnitOfWork 
        => _unitOfWork ??= EngineContext.Current.Resolve<IUnitOfWork<TDbContext>>() 
        ?? throw new ArgumentNullException($"Cannot resolve service of type {typeof(IUnitOfWork<TDbContext>).Name}.");
}
