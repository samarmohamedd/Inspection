using Inspection.Application.Dto;

namespace Inspection.Application.Abstractions
{
    public interface IInspectionVisitService
    {
        Task<PagedResultDto<InspectionVisitDto>> GetAllAsync(InspectionVisitFilterDto filter);
        Task<InspectionVisitDto?> GetByIdAsync(int id);
        Task<InspectionVisitDto> CreateAsync(CreateInspectionVisitDto createInspectionVisitDto);
        Task<InspectionVisitDto?> UpdateAsync(int id, UpdateInspectionVisitDto updateInspectionVisitDto);
        Task<InspectionVisitDto?> CompleteAsync(int id, CompleteInspectionVisitDto completeInspectionVisitDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<InspectionVisitDto>> GetByInspectorAsync(int inspectorId);
    }
}
