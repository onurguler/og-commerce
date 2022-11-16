using Microsoft.EntityFrameworkCore;
using Og.Commerce.Core.Data.DbContexts;
using Og.Commerce.Domain.Localization;

namespace Og.Commerce.Data.DbContexts;

public class ApplicationDbContext : OgDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer("Data Source=localhost;initial catalog=OgCommerce;persist security info=True;Integrated Security=SSPI;MultipleActiveResultSets=True;application name=OgCommerce;trustServerCertificate=true");
    }
    public DbSet<TbLanguage> TbLanguages => Set<TbLanguage>();
}
