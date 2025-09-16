using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Inspection.Application.Dto;
using Inspection.Application.Features.Inspectors.Queries;
using Inspection.Application.Features.Inspectors.Commands;

namespace Inspection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InspectorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<InspectorsController> _logger;

        public InspectorsController(IMediator mediator, ILogger<InspectorsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<InspectorDto>>> GetAll()
        {
            var currentUser = User.Identity?.Name ?? "Unknown";


            try
            {
                var query = new GetAllInspectorsQuery();
                var inspectors = await _mediator.Send(query);

                _logger.LogInformation("Successfully retrieved {InspectorCount} inspectors for admin {AdminUser} in {ElapsedMs}ms",
                    inspectors.Count(), currentUser);

                return Ok(inspectors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inspectors for admin {AdminUser} after {ElapsedMs}ms",
                    currentUser);
                return StatusCode(500, new { message = "An error occurred while retrieving inspectors" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InspectorDto>> GetById(int id)
        {
            var currentUser = User.Identity?.Name ?? "Unknown";
            try
            {
                var query = new GetInspectorByIdQuery(id);
                var inspector = await _mediator.Send(query);

                if (inspector == null)
                {
                    _logger.LogWarning("Inspector {InspectorId} not found for user {CurrentUser} after {ElapsedMs}ms",
                        id, currentUser);
                    return NotFound(new { message = "Inspector not found" });
                }

                _logger.LogInformation("Successfully retrieved inspector {InspectorId} ({InspectorEmail}) for user {CurrentUser} in {ElapsedMs}ms",
                    id, inspector.Email, currentUser);

                return Ok(inspector);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inspector {InspectorId} for user {CurrentUser} after {ElapsedMs}ms",
                    id, currentUser);
                return StatusCode(500, new { message = "An error occurred while retrieving the inspector" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task Create([FromBody] CreateInspectorDto createInspectorDto)
        {
            try
            {
                var command = new CreateInspectorCommand(createInspectorDto);
                 await _mediator.Send(command);
                _logger.LogInformation("Inspector created: {Email}", createInspectorDto.Email);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to create inspector {Email}: {Message}", createInspectorDto.Email, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating inspector {Email}", createInspectorDto.Email);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<InspectorDto>> Update(int id, [FromBody] UpdateInspectorDto updateInspectorDto)
        {
            try
            {
                var command = new UpdateInspectorCommand(id, updateInspectorDto);
                var inspector = await _mediator.Send(command);
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteInspectorCommand(id);
                var result = await _mediator.Send(command);
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
