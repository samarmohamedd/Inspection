using Inspection.Domain.Enum;

namespace Inspection.Domain.Entities
{
    public class EntityToInspect : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public EntityCategory Category { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<InspectionVisit> InspectionVisits { get; set; } = new List<InspectionVisit>();
    }
}
