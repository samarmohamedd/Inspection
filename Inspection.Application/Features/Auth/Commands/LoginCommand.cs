using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.Auth.Commands
{
    public record LoginCommand : IRequest<LoginResponseDto>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public LoginCommand(LoginDto loginDto)
        {
            Email = loginDto.Email;
            Password = loginDto.Password;
        }
    }
}
