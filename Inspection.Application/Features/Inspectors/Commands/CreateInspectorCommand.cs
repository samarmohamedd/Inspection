using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.Inspectors.Commands
{
    public class CreateInspectorCommand : IRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public CreateInspectorCommand(CreateInspectorDto createInspectorDto)
        {
            FullName = createInspectorDto.FullName;
            Email = createInspectorDto.Email;
            Phone = createInspectorDto.Phone;
            RoleId = createInspectorDto.RoleId;
            Password = createInspectorDto.Password;
        }
    }
}
