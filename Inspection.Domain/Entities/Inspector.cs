namespace Inspection.Domain.Entities
{
    public class Inspector : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<InspectionVisit> InspectionVisits { get; set; } = new List<InspectionVisit>();
    }
}
