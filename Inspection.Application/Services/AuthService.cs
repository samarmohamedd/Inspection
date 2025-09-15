using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Abstractions;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Entities;

namespace Inspection.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var inspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (inspector == null || !VerifyPassword(loginDto.Password, inspector.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            if (!inspector.IsActive)
            {
                throw new UnauthorizedAccessException("Account is deactivated");
            }

            var inspectorDto = _mapper.Map<InspectorDto>(inspector);
            var token = GenerateJwtToken(inspectorDto);

            return new LoginResponseDto
            {
                Token = token,
                Inspector = inspectorDto
            };
        }

        public async Task<InspectorDto> RegisterAsync(CreateInspectorDto createInspectorDto)
        {
            var existingInspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .FirstOrDefaultAsync(x => x.Email == createInspectorDto.Email);
            if (existingInspector != null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            var inspector = _mapper.Map<Inspector>(createInspectorDto);
            inspector.PasswordHash = HashPassword(createInspectorDto.Password);

            await _unitOfWork.Inspectors.AddAsync(inspector);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<InspectorDto>(inspector);
        }

        public string GenerateJwtToken(InspectorDto inspector)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "DefaultSecretKeyForDevelopment123456789");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, inspector.Id.ToString()),
                new Claim(ClaimTypes.Name, inspector.FullName),
                new Claim(ClaimTypes.Email, inspector.Email),
                new Claim(ClaimTypes.Role, inspector.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"] ?? "InspectionAPI",
                Audience = jwtSettings["Audience"] ?? "InspectionClient"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
