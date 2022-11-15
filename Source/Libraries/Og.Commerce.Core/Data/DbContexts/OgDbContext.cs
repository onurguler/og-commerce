using Microsoft.EntityFrameworkCore;

namespace Og.Commerce.Core.Data.DbContexts;

public abstract class OgDbContext : DbContext
{
    protected OgDbContext(DbContextOptions options) : base(options) { }
}
