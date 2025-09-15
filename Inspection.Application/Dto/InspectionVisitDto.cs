using Inspection.Domain.Enum;

namespace Inspection.Application.Dto
{
    public class InspectionVisitDto
    {
        public int Id { get; set; }
        public int EntityToInspectId { get; set; }
        public int InspectorId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public InspectionStatus Status { get; set; }
        public int? Score { get; set; }
        public string? Notes { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public EntityToInspectDto? EntityToInspect { get; set; }
        public InspectorDto? Inspector { get; set; }
        public List<ViolationDto> Violations { get; set; } = new List<ViolationDto>();
    }

    public class CreateInspectionVisitDto
    {
        public int EntityToInspectId { get; set; }
        public int InspectorId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateInspectionVisitDto
    {
        public int EntityToInspectId { get; set; }
        public int InspectorId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public InspectionStatus Status { get; set; }
        public int? Score { get; set; }
        public string? Notes { get; set; }
    }

    public class CompleteInspectionVisitDto
    {
        public int Score { get; set; }
        public string? Notes { get; set; }
        public List<CreateViolationDto> Violations { get; set; } = new List<CreateViolationDto>();
    }

    public class InspectionVisitFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public InspectionStatus? Status { get; set; }
        public int? InspectorId { get; set; }
        public string? Category { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
