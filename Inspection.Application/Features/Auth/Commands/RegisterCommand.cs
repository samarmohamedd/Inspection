using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.Auth.Commands
{
    public record RegisterCommand : IRequest<UserDto>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public RegisterCommand(CreateUserDto createUserDto)
        {
            FullName = createUserDto.FullName;
            Email = createUserDto.Email;
            Phone = createUserDto.Phone;
            Password = createUserDto.Password;
            RoleId = createUserDto.RoleId;
        }
    }
}
