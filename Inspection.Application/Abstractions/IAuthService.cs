using Inspection.Application.Dto;

namespace Inspection.Application.Abstractions
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(CreateUserDto createUserDto);
        string GenerateJwtToken(UserDto yser);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
