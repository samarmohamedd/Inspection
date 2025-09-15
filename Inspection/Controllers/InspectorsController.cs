using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inspection.Application.Abstractions;
using Inspection.Application.Dto;

namespace Inspection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InspectorsController : ControllerBase
    {
        private readonly IInspectorService _inspectorService;
        private readonly ILogger<InspectorsController> _logger;

        public InspectorsController(IInspectorService inspectorService, ILogger<InspectorsController> logger)
        {
            _inspectorService = inspectorService;
            _logger = logger;
        }

        /// <summary>
        /// Get all inspectors
        /// </summary>
        /// <returns>List of inspectors</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<InspectorDto>>> GetAll()
        {
            try
            {
                var inspectors = await _inspectorService.GetAllAsync();
                return Ok(inspectors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inspectors");
                return StatusCode(500, new { message = "An error occurred while retrieving inspectors" });
            }
        }

        /// <summary>
        /// Get inspector by ID
        /// </summary>
        /// <param name="id">Inspector ID</param>
        /// <returns>Inspector details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<InspectorDto>> GetById(int id)
        {
            try
            {
                var inspector = await _inspectorService.GetByIdAsync(id);
                if (inspector == null)
                {
                    return NotFound(new { message = "Inspector not found" });
                }
                return Ok(inspector);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inspector {Id}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the inspector" });
            }
        }

        /// <summary>
        /// Create a new inspector
        /// </summary>
        /// <param name="createInspectorDto">Inspector creation data</param>
        /// <returns>Created inspector</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<InspectorDto>> Create([FromBody] CreateInspectorDto createInspectorDto)
        {
            try
            {
                var inspector = await _inspectorService.CreateAsync(createInspectorDto);
                _logger.LogInformation("Inspector created: {Email}", createInspectorDto.Email);
                return CreatedAtAction(nameof(GetById), new { id = inspector.Id }, inspector);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to create inspector {Email}: {Message}", createInspectorDto.Email, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating inspector {Email}", createInspectorDto.Email);
                return StatusCode(500, new { message = "An error occurred while creating the inspector" });
            }
        }

        /// <summary>
        /// Update an inspector
        /// </summary>
        /// <param name="id">Inspector ID</param>
        /// <param name="updateInspectorDto">Inspector update data</param>
        /// <returns>Updated inspector</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<InspectorDto>> Update(int id, [FromBody] UpdateInspectorDto updateInspectorDto)
        {
            try
            {
                var inspector = await _inspectorService.UpdateAsync(id, updateInspectorDto);
                if (inspector == null)
                {
                    return NotFound(new { message = "Inspector not found" });
                }
                _logger.LogInformation("Inspector updated: {Id}", id);
                return Ok(inspector);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to update inspector {Id}: {Message}", id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating inspector {Id}", id);
                return StatusCode(500, new { message = "An error occurred while updating the inspector" });
            }
        }

        /// <summary>
        /// Delete an inspector
        /// </summary>
        /// <param name="id">Inspector ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _inspectorService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Inspector not found" });
                }
                _logger.LogInformation("Inspector deleted: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting inspector {Id}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the inspector" });
            }
        }
    }
}
