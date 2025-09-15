using Inspection.Domain.Enum;

namespace Inspection.Application.Dto
{
    public class EntityToInspectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public EntityCategory Category { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateEntityToInspectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public EntityCategory Category { get; set; }
    }

    public class UpdateEntityToInspectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public EntityCategory Category { get; set; }
        public bool IsActive { get; set; }
    }
}
