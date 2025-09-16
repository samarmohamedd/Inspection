using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Inspection.Application.Dto;
using Inspection.Application.Features.Entities.Queries;
using Inspection.Application.Features.Entities.Commands;
using Inspection.Domain.Constants;

namespace Inspection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EntitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EntitiesController> _logger;

        public EntitiesController(IMediator mediator, ILogger<EntitiesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntityToInspectDto>>> GetAll()
        {
            try
            {
                var query = new GetAllEntitiesQuery();
                var entities = await _mediator.Send(query);
                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entities");
                return StatusCode(500, new { message = "An error occurred while retrieving entities" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EntityToInspectDto>> GetById(int id)
        {
            try
            {
                var query = new GetEntityByIdQuery(id);
                var entity = await _mediator.Send(query);
                if (entity == null)
                {
                    return NotFound(new { message = "Entity not found" });
                }
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entity {Id}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the entity" });
            }
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            try
            {
                var query = new GetCategoriesQuery();
                var categories = await _mediator.Send(query);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                return StatusCode(500, new { message = "An error occurred while retrieving categories" });
            }
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Names.Admin)]
        public async Task<ActionResult<EntityToInspectDto>> Create([FromBody] CreateEntityToInspectDto createEntityDto)
        {
            try
            {
                var command = new CreateEntityCommand(createEntityDto);
                var entity = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating entity {Name}", createEntityDto.Name);
                return StatusCode(500, new { message = "An error occurred while creating the entity" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EntityToInspectDto>> Update(int id, [FromBody] UpdateEntityToInspectDto updateEntityDto)
        {
            try
            {
                var command = new UpdateEntityCommand(id, updateEntityDto);
                var entity = await _mediator.Send(command);
                if (entity == null)
                {
                    return NotFound(new { message = "Entity not found" });
                }
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity {Id}", id);
                return StatusCode(500, new { message = "An error occurred while updating the entity" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteEntityCommand(id);
                var result = await _mediator.Send(command);
                if (!result)
                {
                    return NotFound(new { message = "Entity not found" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity {Id}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the entity" });
            }
        }
    }
}
