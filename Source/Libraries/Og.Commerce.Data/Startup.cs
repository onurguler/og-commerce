using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Og.Commerce.Core.Data.Uow;
using Og.Commerce.Data.DbContexts;
using System.Diagnostics;

namespace Og.Commerce.Data;

public static class Startup
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<DatabaseConfig>()
            .BindConfiguration(nameof(DatabaseConfig))
            .PostConfigure(databaseConfig =>
            {
                Debug.WriteLine($"Current DB Provider: MSSQL");
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services
            .AddDbContext<ApplicationDbContext>((provider, options) =>
            {
                var databaseSettings = provider.GetRequiredService<IOptionsSnapshot<DatabaseConfig>>().Value;
                options.UseSqlServer(databaseSettings.ConnectionString);
            })
            .AddUnitOfWork<ApplicationDbContext>();
      
    }
}
