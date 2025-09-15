using MediatR;
using Inspection.Application.Dto;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.Inspectors.Commands
{
    public class CreateInspectorCommand : IRequest<InspectorDto>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string Password { get; set; } = string.Empty;

        public CreateInspectorCommand(CreateInspectorDto createInspectorDto)
        {
            FullName = createInspectorDto.FullName;
            Email = createInspectorDto.Email;
            Phone = createInspectorDto.Phone;
            Role = createInspectorDto.Role;
            Password = createInspectorDto.Password;
        }
    }
}
