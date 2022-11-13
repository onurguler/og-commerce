using Microsoft.EntityFrameworkCore;
using Og.Commerce.Domain.Localization;

namespace Og.Commerce.Infrastructure.Persistence.DataContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {

    }
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer("Server=localhost;Database=OgCommerce;Trusted_Connection=True;");
    }
    public DbSet<TbLanguage> TbLanguages => Set<TbLanguage>();
}
