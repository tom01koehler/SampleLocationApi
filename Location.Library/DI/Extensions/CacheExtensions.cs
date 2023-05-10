using LocationLibrary.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace LocationLibrary.DI.Extensions
{
    [ExcludeFromCodeCoverage(Justification = "DI Extension")]
    public static class CacheExtensions
    {
        public static IServiceCollection AddCache<T>(this IServiceCollection services) where T : class, ICache
        {
            return services.AddMemoryCache()
                .AddSingleton<ICache, T>();
        }
    }
}
