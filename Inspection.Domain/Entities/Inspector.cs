using Inspection.Domain.Enum;

namespace Inspection.Domain.Entities
{
    public class Inspector : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<InspectionVisit> InspectionVisits { get; set; } = new List<InspectionVisit>();
    }
}
