using LocationLibrary.Contracts.Dtos;
using LocationLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService) 
        {
            _locationService = locationService;
        }

        [HttpPost]
        public async Task<ActionResult<LocationDto>> Create(string name, string address)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest($"{nameof(name)} is required.");

            try
            {
                var location = await _locationService.AddLocation(name, address);
                return Ok(location);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDto>> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) 
                return BadRequest();

            try
            {
                var location = await _locationService.GetLocation(id);

                if (location is null)
                    return NotFound($"{nameof(id)} {id} is not found.");

                return Ok(location);
            }
            catch (ArgumentNullException ex) 
            { 
                return  BadRequest(ex.Message); 
            }
        }

        [HttpGet("locations")]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetAll()
        {
            var locations = await _locationService.GetAllLocations();

            return Ok(locations);
        }

        [HttpPut]
        public async Task<ActionResult<LocationDto>> Put(LocationDto location)
        {
            if (string.IsNullOrWhiteSpace(location.Id) || string.IsNullOrWhiteSpace(location.Name))
                return BadRequest($"{nameof(location.Id)} and {nameof(location.Name)} are required.");

            try
            {
                location = await _locationService.UpdateLocation(location);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateException updEx)
            {
                return Conflict(updEx.Message);
            }

            return Ok(location);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest($"{nameof(id)} is required.");

            try
            {
                await _locationService.DeleteLocation(id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
