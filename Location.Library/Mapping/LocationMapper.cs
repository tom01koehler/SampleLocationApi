using LocationLibrary.Contracts.Dtos;
using LocationLibrary.Contracts.Models;

namespace LocationLibrary.Mapping
{
    public static class LocationMapper
    {
        public static LocationDto ToDto(this Location location)
        {
            return new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
            };
        }

        public static Location UpdateData(this Location data, Location updates)
        {
            data.Name = updates.Name;
            data.Address = updates.Address;

            return data;
        }

        public static Location ToDataModel(this LocationDto location)
        {
            return new Location
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
            };
        }

        public static ICollection<LocationDto> ToDto(this ICollection<Location> locations)
        {
            return locations.Select(l => l.ToDto()).ToList();
        }
    }
}
