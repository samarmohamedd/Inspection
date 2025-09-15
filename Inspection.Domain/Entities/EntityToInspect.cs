namespace Inspection.Domain.Entities
{
    public class EntityToInspect : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<InspectionVisit> InspectionVisits { get; set; } = new List<InspectionVisit>();
    }
}
