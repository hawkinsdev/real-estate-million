using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Modules.Owner.DTOs;
using RealEstate.Application.Modules.Owner.Interfaces;
using System.Net;

namespace RealEstate.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OwnersController(IOwnerService propertyService, ILogger<OwnersController> logger) : ControllerBase
    {
        private readonly IOwnerService _ownerService = propertyService ?? throw new ArgumentNullException(nameof(propertyService));
        private readonly ILogger<OwnersController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Retrieves all owners with their properties
        /// </summary>
        /// <returns>A list of owners with their properties</returns>
        [HttpGet("with-properties")]
        [ProducesResponseType(typeof(IEnumerable<OwnerWithPropertiesDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<OwnerWithPropertiesDto>>> GetAllOwnersWithProperties()
        {
            try
            {
                _logger.LogInformation("Getting all owners with their properties");
                var owners = await _ownerService.GetAllOwnersWithPropertiesAsync();
                return Ok(owners);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all owners with properties");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Retrieves a specific owner by ID with properties
        /// </summary>
        /// <param name="id">The owner ID</param>
        /// <returns>The owner details with properties</returns>
        [HttpGet("{id}/with-properties")]
        [ProducesResponseType(typeof(OwnerWithPropertiesDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<OwnerWithPropertiesDto>> GetOwnerWithPropertiesById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Owner ID is required");
            }

            try
            {
                _logger.LogInformation("Getting owner with properties for ID: {OwnerId}", id);
                var owner = await _ownerService.GetOwnerWithPropertiesByIdAsync(id);

                if (owner == null)
                {
                    _logger.LogWarning("Owner with ID {OwnerId} not found", id);
                    return NotFound($"Owner with ID {id} not found");
                }

                return Ok(owner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting owner with properties for ID: {OwnerId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

    }
}