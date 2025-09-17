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
                return CreatedAtAction(nameof(GetById), new { id = visit.Id }, visit);
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
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var command = new UpdateVisitCommand(id, updateVisitDto);
                var visit = await _mediator.Send(command);
                if (visit == null)
                {
                    return NotFound(new { message = "Inspection visit not found" });
                }
                return Ok(visit);
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
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var command = new CompleteVisitCommand(id, completeVisitDto);
                var visit = await _mediator.Send(command);
                if (visit == null)
                {
                    return NotFound(new { message = "Inspection visit not found" });
                }
                return Ok(visit);
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
                return NoContent();
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
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string. Empty;
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
