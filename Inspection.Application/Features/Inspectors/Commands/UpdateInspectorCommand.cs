using MediatR;
using Inspection.Application.Dto;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.Inspectors.Commands
{
    public class UpdateInspectorCommand : IRequest<InspectorDto?>
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }

        public UpdateInspectorCommand(int id, UpdateInspectorDto updateInspectorDto)
        {
            Id = id;
            FullName = updateInspectorDto.FullName;
            Email = updateInspectorDto.Email;
            Phone = updateInspectorDto.Phone;
            Role = updateInspectorDto.Role;
            IsActive = updateInspectorDto.IsActive;
        }
    }
}
