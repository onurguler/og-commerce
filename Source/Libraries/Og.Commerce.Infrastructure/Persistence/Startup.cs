using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Og.Commerce.Infrastructure.Persistence.DataContexts;
using System.Diagnostics;

namespace Og.Commerce.Infrastructure.Persistence;

internal static class Startup
{
    internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config) 
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
            });
    }
}
