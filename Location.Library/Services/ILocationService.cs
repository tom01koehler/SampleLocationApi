using LocationLibrary.Contracts.Dtos;

namespace LocationLibrary.Services
{
    public interface ILocationService
    {
        /// <summary>
        /// Adds location.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public Task<LocationDto> AddLocation(string name, string address);

        /// <summary>
        /// Deletes location.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteLocation(string id);

        /// <summary>
        /// Gets all locations.
        /// </summary>
        /// <returns></returns>
        public Task<ICollection<LocationDto>> GetAllLocations();

        /// <summary>
        /// Updates location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public Task<LocationDto> UpdateLocation(LocationDto location);

        /// <summary>
        /// Gets location.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<LocationDto> GetLocation(string id);
    }
}
