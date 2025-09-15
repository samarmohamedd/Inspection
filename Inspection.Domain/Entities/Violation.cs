using Inspection.Domain.Enum;

namespace Inspection.Domain.Entities
{
    public class Violation : BaseEntity
    {
        public int InspectionVisitId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ViolationSeverity Severity { get; set; }
        public virtual InspectionVisit InspectionVisit { get; set; } = null!;
    }
}
