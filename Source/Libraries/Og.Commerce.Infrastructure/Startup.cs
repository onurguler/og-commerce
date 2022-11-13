using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Og.Commerce.Infrastructure.Persistence;

namespace Og.Commerce.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        return services.AddPersistence(config);
    }
}
