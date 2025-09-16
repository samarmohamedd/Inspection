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
                    return NotFound(new { message = "Inspector not found" });
                }
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
                return Ok(inspector);
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
