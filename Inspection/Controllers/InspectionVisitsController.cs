using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;
using Inspection.Application.Dto;
using Inspection.Domain.Enum;
using Inspection.Application.Features.InspectionVisits.Queries;
using Inspection.Application.Features.InspectionVisits.Commands;

namespace Inspection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InspectionVisitsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<InspectionVisitsController> _logger;

        public InspectionVisitsController(IMediator mediator, ILogger<InspectionVisitsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

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

                var query = new GetAllVisitsQuery(filter);
                var visits = await _mediator.Send(query);
                return Ok(visits);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inspection visits");
                return StatusCode(500, new { message = "An error occurred while retrieving inspection visits" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InspectionVisitDto>> GetById(int id)
        {
            try
            {
                var query = new GetVisitByIdQuery(id);
                var visit = await _mediator.Send(query);
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<InspectionVisitDto>> Create([FromBody] CreateInspectionVisitDto createVisitDto)
        {
            try
            {
                var command = new CreateVisitCommand(createVisitDto);
                var visit = await _mediator.Send(command);
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
                    var getQuery = new GetVisitByIdQuery(id);
                    var existingVisit = await _mediator.Send(getQuery);
                    if (existingVisit?.InspectorId != userId)
                    {
                        return Forbid();
                    }
                }

                var command = new UpdateVisitCommand(id, updateVisitDto);
                var visit = await _mediator.Send(command);
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
                    var getQuery = new GetVisitByIdQuery(id);
                    var existingVisit = await _mediator.Send(getQuery);
                    if (existingVisit?.InspectorId != userId)
                    {
                        return Forbid();
                    }
                }

                var command = new CompleteVisitCommand(id, completeVisitDto);
                var visit = await _mediator.Send(command);
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteVisitCommand(id);
                var result = await _mediator.Send(command);
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

        [HttpGet("my-visits")]
        [Authorize(Roles = "Inspector")]
        public async Task<ActionResult<IEnumerable<InspectionVisitDto>>> GetMyVisits()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var query = new GetMyVisitsQuery(userId);
                var visits = await _mediator.Send(query);
                return Ok(visits);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inspector visits");
                return StatusCode(500, new { message = "An error occurred while retrieving your visits" });
            }
        }

        [HttpGet("dashboard")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DashboardDto>> GetDashboard()
        {
            try
            {
                var query = new GetDashboardQuery();
                var dashboard = await _mediator.Send(query);
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard data");
                return StatusCode(500, new { message = "An error occurred while retrieving dashboard data" });
            }
        }
    }
}
