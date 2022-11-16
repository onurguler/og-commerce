using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Og.Commerce.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services) 
        => services.AddAutoMapper(Assembly.GetExecutingAssembly());
}
