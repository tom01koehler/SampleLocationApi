namespace LocationLibrary.Repos
{
    /// <summary>
    /// Data access for CRUD operations of type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepo<T> where T : class
    {
        /// <summary>
        /// Creates a record of type <see cref="T"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <returns><see cref="Task{T}"/></returns>
        public Task<T> Create(T data);

        /// <summary>
        /// Updates data for provided <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns><see cref="Task{T}"/></returns>
        public Task<T> Update(T data);

        /// <summary>
        /// Deletes data of <see cref="T"/> by provided id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns><see cref="Task"/></returns>
        public Task Delete(string id);

        /// <summary>
        /// Retrieves all data of <see cref="T"/> from data store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><see cref="Task{ICollection{T}}"/></returns>
        public Task<ICollection<T>> GetAll();

        /// <summary>
        /// Retrieves data by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="Task{Task}"/></returns>
        public Task<T> Get(string id);
    }
}
