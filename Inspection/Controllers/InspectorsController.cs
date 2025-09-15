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
            try
            {
                var query = new GetAllInspectorsQuery();
                var inspectors = await _mediator.Send(query);
                return Ok(inspectors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inspectors");
                return StatusCode(500, new { message = "An error occurred while retrieving inspectors" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InspectorDto>> GetById(int id)
        {
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
                _logger.LogError(ex, "Error retrieving inspector {Id}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the inspector" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<InspectorDto>> Create([FromBody] CreateInspectorDto createInspectorDto)
        {
            try
            {
                var command = new CreateInspectorCommand(createInspectorDto);
                var inspector = await _mediator.Send(command);
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
