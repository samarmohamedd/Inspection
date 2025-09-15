using Inspection.Application.Dto;

namespace Inspection.Application.Abstractions
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<InspectorDto> RegisterAsync(CreateInspectorDto createInspectorDto);
        string GenerateJwtToken(InspectorDto inspector);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
