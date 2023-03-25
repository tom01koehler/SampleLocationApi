namespace LocationLibrary.Abstractions
{
    public interface ICache
    {
        /// <summary>
        /// Gets item from cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(object key) where T : class;

        /// <summary>
        /// Sets cache with item by id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set<T>(object key, T value) where T : class;

        /// <summary>
        /// Removes item from cache.
        /// </summary>
        /// <param name="key"></param>
        void Remove(object key);

        /// <summary>
        /// Gets all items from cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>() where T : class;

    }
}
