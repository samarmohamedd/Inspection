using Inspection.Application.Dto;

namespace Inspection.Application.Abstractions
{
    public interface IInspectorService
    {
        Task<IEnumerable<InspectorDto>> GetAllAsync();
        Task<InspectorDto?> GetByIdAsync(int id);
        Task<InspectorDto?> GetByEmailAsync(string email);
        Task CreateAsync(CreateInspectorDto createInspectorDto);
        Task<InspectorDto?> UpdateAsync(int id, UpdateInspectorDto updateInspectorDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    }
}
