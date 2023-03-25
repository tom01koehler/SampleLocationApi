using LocationLibrary.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LocationLibrary.DI.Extensions
{
    public static class CacheExtensions
    {
        public static IServiceCollection AddCache<T>(this IServiceCollection services) where T : class, ICache
        {
            return services.AddMemoryCache()
                .AddSingleton<ICache, T>();
        }
    }
}
