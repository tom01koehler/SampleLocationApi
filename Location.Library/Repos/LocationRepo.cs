using LocationLibrary.Contracts.Models;
using LocationLibrary.Data;
using LocationLibrary.Mapping;
using Microsoft.EntityFrameworkCore;

namespace LocationLibrary.Repos
{
    /// <inheritdoc/>
    public class LocationRepo : IRepo<Location>
    {
        private readonly LocationDbContext _context;

        public LocationRepo(LocationDbContext dbContext) 
        {
            _context = dbContext; 
        }

        /// <inheritdoc/>
        public async Task<Location> Create(Location data)
        {
            if (string.IsNullOrWhiteSpace(data.Name))
                throw new ArgumentNullException($"{nameof(data.Name)} is required for a location.");

            if (await _context.Locations.AnyAsync(l => l.Name.Equals(data.Name)))
                throw new ArgumentException($"A location with the {nameof(data.Name)} {data.Name} already exists");

            await _context.AddAsync(data);

            await _context.SaveChangesAsync();

            return data;
        }

        /// <inheritdoc/>
        public async Task Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) 
                throw new ArgumentNullException($"{nameof(id)} is required.");

            var existingLocation = await _context.FindAsync<Location>(id);

            if (existingLocation is null)
                throw new KeyNotFoundException($"The location with {nameof(id)} {id} is not in the database.");

            _context.Remove(existingLocation);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Location> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) 
                throw new ArgumentNullException($"{nameof(id)} is required.");

            return await _context.FindAsync<Location>(id);
        }

        /// <inheritdoc/>
        public async Task<ICollection<Location>> GetAll()
        {
            return await _context.Locations.ToListAsync<Location>();
        }

        /// <inheritdoc/>
        public async Task<Location> Update(Location data)
        {
            if (string.IsNullOrWhiteSpace(data.Name))
                throw new ArgumentNullException($"{nameof(data.Name)} is required for a location.");

            var existingLocation = await _context.FindAsync<Location>(data.Id);

            if (existingLocation is null)
                throw new KeyNotFoundException($"The location with {nameof(data.Id)} {data.Id} is not in the database.");

            if (await _context.Locations.AnyAsync<Location>(l => !l.Id.Equals(data.Id) && l.Name.Equals(data.Name)))
                throw new DbUpdateException($"A location with the {nameof(data.Name)} {data.Name} already exists.");

            existingLocation.UpdateData(data);

            await _context.SaveChangesAsync();

            return existingLocation;
        }
    }
}
