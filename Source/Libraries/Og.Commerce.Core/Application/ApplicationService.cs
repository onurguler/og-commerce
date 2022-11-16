using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Og.Commerce.Core.Data.Uow;
using Og.Commerce.Core.Infrastructure;

namespace Og.Commerce.Core.Application;

public abstract class ApplicationService<TDbContext> where TDbContext : DbContext
{
    private IUnitOfWork<TDbContext>? _unitOfWork;
    private IMapper? _mapper;

    public IUnitOfWork<TDbContext> UnitOfWork => _unitOfWork ??= EngineContext.Current.Resolve<IUnitOfWork<TDbContext>>();
    public IMapper ObjectMapper => _mapper ??= EngineContext.Current.Resolve<IMapper>();
}
