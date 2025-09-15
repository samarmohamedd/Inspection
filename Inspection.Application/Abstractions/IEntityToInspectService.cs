using Inspection.Application.Dto;
using Inspection.Domain.Enum;

namespace Inspection.Application.Abstractions
{
    public interface IEntityToInspectService
    {
        Task<IEnumerable<EntityToInspectDto>> GetAllAsync();
        Task<EntityToInspectDto?> GetByIdAsync(int id);
        Task<EntityToInspectDto> CreateAsync(CreateEntityToInspectDto createEntityToInspectDto);
        Task<EntityToInspectDto?> UpdateAsync(int id, UpdateEntityToInspectDto updateEntityToInspectDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<EntityCategory>> GetCategoriesAsync();
    }
}
