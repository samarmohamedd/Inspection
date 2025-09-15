using Inspection.Application.Dto;

namespace Inspection.Application.Abstractions
{
    public interface IViolationService
    {
        Task<IEnumerable<ViolationDto>> GetByInspectionVisitAsync(int inspectionVisitId);
        Task<ViolationDto?> GetByIdAsync(int id);
        Task<ViolationDto> CreateAsync(int inspectionVisitId, CreateViolationDto createViolationDto);
        Task<ViolationDto?> UpdateAsync(int id, UpdateViolationDto updateViolationDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
