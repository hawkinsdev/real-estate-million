using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Modules.Property.DTOs;
using RealEstate.Application.Modules.Property.Interfaces;
using System.Net;

namespace RealEstate.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PropertyController(IPropertyService propertyService, ILogger<PropertyController> logger) : ControllerBase
    {
        private readonly IPropertyService _propertyService = propertyService ?? throw new ArgumentNullException(nameof(propertyService));
        private readonly ILogger<PropertyController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Retrieves all properties
        /// </summary>
        /// <returns>A list of properties</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PropertyDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAllProperties()
        {
            try
            {
                _logger.LogInformation("Getting all properties");
                var properties = await _propertyService.GetAllPropertiesAsync();
                return Ok(properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all properties");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Retrieves properties in simplified format for the technical test
        /// </summary>
        /// <returns>A list of properties in simplified format</returns>
        [HttpGet("simple")]
        [ProducesResponseType(typeof(IEnumerable<PropertySimpleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<PropertySimpleDto>>> GetAllPropertiesSimple()
        {
            try
            {
                _logger.LogInformation("Getting all properties in simple format");
                var filter = new PropertyFilterDto();
                var properties = await _propertyService.GetFilteredPropertiesSimpleAsync(filter);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all properties in simple format");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Retrieves a specific property by ID
        /// </summary>
        /// <param name="id">The property ID</param>
        /// <returns>The property details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PropertyDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<PropertyDto>> GetPropertyById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Property ID is required");
            }

            try
            {
                _logger.LogInformation("Getting property with ID: {PropertyId}", id);
                var property = await _propertyService.GetPropertyByIdAsync(id);

                if (property == null)
                {
                    _logger.LogWarning("Property with ID {PropertyId} not found", id);
                    return NotFound($"Property with ID {id} not found");
                }

                return Ok(property);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting property with ID: {PropertyId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Retrieves properties based on filter criteria
        /// </summary>
        /// <param name="name">Filter by property name (partial match)</param>
        /// <param name="address">Filter by address (partial match)</param>
        /// <param name="minPrice">Minimum price filter</param>
        /// <param name="maxPrice">Maximum price filter</param>
        /// <returns>A filtered list of properties</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<PropertyDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> SearchProperties(
            [FromQuery] string? name = null,
            [FromQuery] string? address = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
            {
                return BadRequest("Minimum price cannot be greater than maximum price");
            }

            if (minPrice.HasValue && minPrice < 0)
            {
                return BadRequest("Minimum price cannot be negative");
            }

            if (maxPrice.HasValue && maxPrice < 0)
            {
                return BadRequest("Maximum price cannot be negative");
            }

            try
            {
                _logger.LogInformation("Searching properties with filters - Name: {Name}, Address: {Address}, MinPrice: {MinPrice}, MaxPrice: {MaxPrice}",
                    name, address, minPrice, maxPrice);

                var filter = new PropertyFilterDto
                {
                    Name = name,
                    Address = address,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };

                var properties = await _propertyService.GetFilteredPropertiesAsync(filter);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching properties");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Retrieves properties in simplified format based on filter criteria
        /// </summary>
        /// <param name="name">Filter by property name (partial match)</param>
        /// <param name="address">Filter by address (partial match)</param>
        /// <param name="minPrice">Minimum price filter</param>
        /// <param name="maxPrice">Maximum price filter</param>
        /// <returns>A filtered list of properties in simplified format</returns>
        [HttpGet("search/simple")]
        [ProducesResponseType(typeof(IEnumerable<PropertySimpleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<PropertySimpleDto>>> SearchPropertiesSimple(
            [FromQuery] string? name = null,
            [FromQuery] string? address = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
            {
                return BadRequest("Minimum price cannot be greater than maximum price");
            }

            if (minPrice.HasValue && minPrice < 0)
            {
                return BadRequest("Minimum price cannot be negative");
            }

            if (maxPrice.HasValue && maxPrice < 0)
            {
                return BadRequest("Maximum price cannot be negative");
            }

            try
            {
                _logger.LogInformation("Searching properties in simple format with filters - Name: {Name}, Address: {Address}, MinPrice: {MinPrice}, MaxPrice: {MaxPrice}",
                    name, address, minPrice, maxPrice);

                var filter = new PropertyFilterDto
                {
                    Name = name,
                    Address = address,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };

                var properties = await _propertyService.GetFilteredPropertiesSimpleAsync(filter);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching properties in simple format");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Creates a new property
        /// </summary>
        /// <param name="createPropertyDto">Property data to create</param>
        /// <returns>The created property</returns>
        [HttpPost]
        [ProducesResponseType(typeof(PropertyDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<PropertyDto>> CreateProperty([FromBody] CreatePropertyDto createPropertyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating new property with name: {PropertyName}", createPropertyDto.Name);
                var createdProperty = await _propertyService.CreatePropertyAsync(createPropertyDto);

                return CreatedAtAction(
                    nameof(GetPropertyById),
                    new { id = createdProperty.IdProperty },
                    createdProperty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating property");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Updates an existing property
        /// </summary>
        /// <param name="id">The property ID to update</param>
        /// <param name="updatePropertyDto">Updated property data</param>
        /// <returns>No content if successful</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateProperty(string id, [FromBody] UpdatePropertyDto updatePropertyDto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Property ID is required");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Updating property with ID: {PropertyId}", id);
                await _propertyService.UpdatePropertyAsync(id, updatePropertyDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Property with ID {PropertyId} not found for update", id);
                return NotFound($"Property with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating property with ID: {PropertyId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Deletes a property
        /// </summary>
        /// <param name="id">The property ID to delete</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteProperty(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Property ID is required");
            }

            try
            {
                _logger.LogInformation("Deleting property with ID: {PropertyId}", id);
                await _propertyService.DeletePropertyAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Property with ID {PropertyId} not found for deletion", id);
                return NotFound($"Property with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting property with ID: {PropertyId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Checks if a property exists
        /// </summary>
        /// <param name="id">The property ID to check</param>
        /// <returns>Boolean indicating if the property exists</returns>
        [HttpHead("{id}")]
        [HttpGet("{id}/exists")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PropertyExists(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Property ID is required");
            }

            try
            {
                var exists = await _propertyService.PropertyExistsAsync(id);
                return exists ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if property exists with ID: {PropertyId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
