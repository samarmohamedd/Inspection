using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Inspection.Application.Abstractions;
using Inspection.Application.Dto;
using Inspection.Domain.Enum;

namespace Inspection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InspectionVisitsController : ControllerBase
    {
        private readonly IInspectionVisitService _visitService;
        private readonly ILogger<InspectionVisitsController> _logger;

        public InspectionVisitsController(IInspectionVisitService visitService, ILogger<InspectionVisitsController> logger)
        {
            _visitService = visitService;
            _logger = logger;
        }

        /// <summary>
        /// Get all inspection visits with filtering
        /// </summary>
        /// <param name="filter">Filter parameters</param>
        /// <returns>Paginated list of inspection visits</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResultDto<InspectionVisitDto>>> GetAll([FromQuery] InspectionVisitFilterDto filter)
        {
            try
            {
                // If user is Inspector, only show their visits
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userRole == UserRole.Inspector.ToString())
                {
                    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                    filter.InspectorId = userId;
                }

                var visits = await _visitService.GetAllAsync(filter);
                return Ok(visits);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inspection visits");
                return StatusCode(500, new { message = "An error occurred while retrieving inspection visits" });
            }
        }

        /// <summary>
        /// Get inspection visit by ID
        /// </summary>
        /// <param name="id">Visit ID</param>
        /// <returns>Visit details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<InspectionVisitDto>> GetById(int id)
        {
            try
            {
                var visit = await _visitService.GetByIdAsync(id);
                if (visit == null)
                {
                    return NotFound(new { message = "Inspection visit not found" });
                }

                // Check if user has access to this visit
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userRole == UserRole.Inspector.ToString())
                {
                    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                    if (visit.InspectorId != userId)
                    {
                        return Forbid();
                    }
                }

                return Ok(visit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inspection visit {Id}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the inspection visit" });
            }
        }

        /// <summary>
        /// Create a new inspection visit
        /// </summary>
        /// <param name="createVisitDto">Visit creation data</param>
        /// <returns>Created visit</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<InspectionVisitDto>> Create([FromBody] CreateInspectionVisitDto createVisitDto)
        {
            try
            {
                var visit = await _visitService.CreateAsync(createVisitDto);
                _logger.LogInformation("Inspection visit created: {Id}", visit.Id);
                return CreatedAtAction(nameof(GetById), new { id = visit.Id }, visit);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to create inspection visit: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating inspection visit");
                return StatusCode(500, new { message = "An error occurred while creating the inspection visit" });
            }
        }

        /// <summary>
        /// Update an inspection visit
        /// </summary>
        /// <param name="id">Visit ID</param>
        /// <param name="updateVisitDto">Visit update data</param>
        /// <returns>Updated visit</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<InspectionVisitDto>> Update(int id, [FromBody] UpdateInspectionVisitDto updateVisitDto)
        {
            try
            {
                // Check if user has access to this visit
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userRole == UserRole.Inspector.ToString())
                {
                    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                    var existingVisit = await _visitService.GetByIdAsync(id);
                    if (existingVisit?.InspectorId != userId)
                    {
                        return Forbid();
                    }
                }

                var visit = await _visitService.UpdateAsync(id, updateVisitDto);
                if (visit == null)
                {
                    return NotFound(new { message = "Inspection visit not found" });
                }
                _logger.LogInformation("Inspection visit updated: {Id}", id);
                return Ok(visit);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to update inspection visit {Id}: {Message}", id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating inspection visit {Id}", id);
                return StatusCode(500, new { message = "An error occurred while updating the inspection visit" });
            }
        }

        /// <summary>
        /// Complete an inspection visit
        /// </summary>
        /// <param name="id">Visit ID</param>
        /// <param name="completeVisitDto">Completion data</param>
        /// <returns>Completed visit</returns>
        [HttpPost("{id}/complete")]
        public async Task<ActionResult<InspectionVisitDto>> Complete(int id, [FromBody] CompleteInspectionVisitDto completeVisitDto)
        {
            try
            {
                // Check if user has access to this visit
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userRole == UserRole.Inspector.ToString())
                {
                    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                    var existingVisit = await _visitService.GetByIdAsync(id);
                    if (existingVisit?.InspectorId != userId)
                    {
                        return Forbid();
                    }
                }

                var visit = await _visitService.CompleteAsync(id, completeVisitDto);
                if (visit == null)
                {
                    return NotFound(new { message = "Inspection visit not found" });
                }
                _logger.LogInformation("Inspection visit completed: {Id}", id);
                return Ok(visit);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to complete inspection visit {Id}: {Message}", id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing inspection visit {Id}", id);
                return StatusCode(500, new { message = "An error occurred while completing the inspection visit" });
            }
        }

        /// <summary>
        /// Delete an inspection visit
        /// </summary>
        /// <param name="id">Visit ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _visitService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Inspection visit not found" });
                }
                _logger.LogInformation("Inspection visit deleted: {Id}", id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to delete inspection visit {Id}: {Message}", id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting inspection visit {Id}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the inspection visit" });
            }
        }

        /// <summary>
        /// Get visits assigned to current inspector
        /// </summary>
        /// <returns>List of visits for current inspector</returns>
        [HttpGet("my-visits")]
        [Authorize(Roles = "Inspector")]
        public async Task<ActionResult<IEnumerable<InspectionVisitDto>>> GetMyVisits()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var visits = await _visitService.GetByInspectorAsync(userId);
                return Ok(visits);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inspector visits");
                return StatusCode(500, new { message = "An error occurred while retrieving your visits" });
            }
        }
    }
}
