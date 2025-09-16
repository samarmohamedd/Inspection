namespace Inspection.Domain.Entities
{
    public class Inspector : BaseEntity
    {
        public string UserId { get; set; } = string.Empty; // Foreign key to ApplicationUser
        public string RoleId { get; set; } = string.Empty; // Foreign key to ApplicationRole
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ApplicationRole Role { get; set; } = null!;
        public virtual ICollection<InspectionVisit> InspectionVisits { get; set; } = new List<InspectionVisit>();
    }
}
