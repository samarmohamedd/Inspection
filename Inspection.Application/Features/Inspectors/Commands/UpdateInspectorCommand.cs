using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.Inspectors.Commands
{
    public record UpdateInspectorCommand : IRequest<InspectorDto?>
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public UpdateInspectorCommand(int id, UpdateInspectorDto updateInspectorDto)
        {
            Id = id;
            FullName = updateInspectorDto.FullName;
            Email = updateInspectorDto.Email;
            Phone = updateInspectorDto.Phone;
            IsActive = updateInspectorDto.IsActive;
        }
    }
}
