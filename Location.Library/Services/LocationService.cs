using LocationLibrary.Abstractions;
using LocationLibrary.Contracts.Dtos;
using LocationLibrary.Contracts.Models;
using LocationLibrary.Mapping;
using LocationLibrary.Repos;

namespace LocationLibrary.Services
{
    public class LocationService : ILocationService
    {
        private readonly IRepo<Location> _locationRepo;
        private readonly ICache _cache;

        public LocationService(
            IRepo<Location> locationRepo,
            ICache cache) 
        { 
            _locationRepo = locationRepo;
            _cache = cache;
        }

        /// <inheritdoc/>
        public async Task<LocationDto> AddLocation(string name, string address)
        {
            var location = new Location
            { 
                Name = name, 
                Address = address 
            };

            location = await _locationRepo.Create(location);

            _cache.Set(location.Id, location);

            return location.ToDto();
        }

        /// <inheritdoc/>
        public async Task DeleteLocation(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException($"{nameof(id)} is required.");

            await _locationRepo.Delete(id);

            _cache.Remove(id);
        }

        /// <inheritdoc/>
        public async Task<ICollection<LocationDto>> GetAllLocations()
        {
            var locationData = _cache.GetAll<Location>();

            if (locationData is null || !locationData.Any()) 
                locationData = await _locationRepo.GetAll();

            return locationData.ToList().ToDto();
        }

        /// <inheritdoc/>
        public async Task<LocationDto> GetLocation(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException($"{nameof(id)} is required.");

            var locationData = _cache.Get<Location>(id);

            if (locationData is null)
                locationData = await _locationRepo.Get(id);

            _cache.Set<Location>(id, locationData);

            return locationData?.ToDto();
        }

        /// <inheritdoc/>
        public async Task<LocationDto> UpdateLocation(LocationDto location)
        {
            if (location is null)
                throw new ArgumentNullException($"{nameof(location)} is required.");

            var locationData = location.ToDataModel();

            var updatedLocation = await _locationRepo.Update(locationData);

            _cache.Set(updatedLocation.Id, updatedLocation);

            return updatedLocation.ToDto();
        }
    }
}
