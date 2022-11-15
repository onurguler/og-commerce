using Microsoft.AspNetCore.Builder;
using Og.Commerce.Core.Infrastructure;

namespace Og.Commerce.Core;

public static class Startup
{
    /// <summary>
    /// Configure the application HTTP request pipeline
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void ConfigureRequestPipeline(this IApplicationBuilder application)
    {
        EngineContext.Current.ConfigureRequestPipeline(application);
    }
}
