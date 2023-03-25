using LocationLibrary.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace LocationApi.Cache
{
    public class Cache : ICache
    {
        private readonly IMemoryCache _cache;

        public Cache(IMemoryCache cache) 
        {
            _cache = cache;
        }

        /// <inheritdoc/>
        public T Get<T>(object key) where T : class
        {
            return _cache.Get<T>(key);
        }

        /// <inheritdoc/>
        public IEnumerable<T> GetAll<T>() where T : class
        {
            var entriesInfo = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var entries = entriesInfo?.GetValue(_cache) as dynamic;

            var items = new List<T>();

            if (entries is not null)
            {
                foreach ( var entry in entries)
                {
                    if (_cache.TryGetValue(entry.GetType().GetProperty("Key").GetValue(entry) as object, out var value))
                    {
                        if (value is not null && value.GetType().Equals(typeof(T)))
                            items.Add(value as T);
                    } 
                }
            }

            return items;
        }

        /// <inheritdoc/>
        public void Remove(object key)
        {
            _cache.Remove(key);
        }

        /// <inheritdoc/>
        public void Set<T>(object key, T value) where T : class
        {
            _cache.Set(key, value);
        }
    }
}
