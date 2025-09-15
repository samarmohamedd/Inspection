using Inspection.Domain.Enum;

namespace Inspection.Domain.Entities
{
    public class InspectionVisit : BaseEntity
    {
        public int EntityToInspectId { get; set; }
        public int InspectorId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public InspectionStatus Status { get; set; } = InspectionStatus.Planned;
        public int? Score { get; set; } // 0-100
        public string? Notes { get; set; }
        public DateTime? CompletedAt { get; set; }
        public virtual EntityToInspect EntityToInspect { get; set; } = null!;
        public virtual Inspector Inspector { get; set; } = null!;
        public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
    }
}
