using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inspection.Application.Abstractions;
using Inspection.Application.Dto;

namespace Inspection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EntitiesController : ControllerBase
    {
        private readonly IEntityToInspectService _entityService;
        private readonly ILogger<EntitiesController> _logger;

        public EntitiesController(IEntityToInspectService entityService, ILogger<EntitiesController> logger)
        {
            _entityService = entityService;
            _logger = logger;
        }

        /// <summary>
        /// Get all entities to inspect
        /// </summary>
        /// <returns>List of entities</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntityToInspectDto>>> GetAll()
        {
            try
            {
                var entities = await _entityService.GetAllAsync();
                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entities");
                return StatusCode(500, new { message = "An error occurred while retrieving entities" });
            }
        }

        /// <summary>
        /// Get entity by ID
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Entity details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EntityToInspectDto>> GetById(int id)
        {
            try
            {
                var entity = await _entityService.GetByIdAsync(id);
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

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of categories</returns>
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            try
            {
                var categories = await _entityService.GetCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                return StatusCode(500, new { message = "An error occurred while retrieving categories" });
            }
        }

        /// <summary>
        /// Create a new entity to inspect
        /// </summary>
        /// <param name="createEntityDto">Entity creation data</param>
        /// <returns>Created entity</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EntityToInspectDto>> Create([FromBody] CreateEntityToInspectDto createEntityDto)
        {
            try
            {
                var entity = await _entityService.CreateAsync(createEntityDto);
                _logger.LogInformation("Entity created: {Name}", createEntityDto.Name);
                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating entity {Name}", createEntityDto.Name);
                return StatusCode(500, new { message = "An error occurred while creating the entity" });
            }
        }

        /// <summary>
        /// Update an entity to inspect
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="updateEntityDto">Entity update data</param>
        /// <returns>Updated entity</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EntityToInspectDto>> Update(int id, [FromBody] UpdateEntityToInspectDto updateEntityDto)
        {
            try
            {
                var entity = await _entityService.UpdateAsync(id, updateEntityDto);
                if (entity == null)
                {
                    return NotFound(new { message = "Entity not found" });
                }
                _logger.LogInformation("Entity updated: {Id}", id);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity {Id}", id);
                return StatusCode(500, new { message = "An error occurred while updating the entity" });
            }
        }

        /// <summary>
        /// Delete an entity to inspect
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _entityService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Entity not found" });
                }
                _logger.LogInformation("Entity deleted: {Id}", id);
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
