using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Inspection.Application.Abstractions;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Entities;
using Inspection.Domain.Enum;
using Inspection.Domain.Constants;

namespace Inspection.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    throw new UnauthorizedAccessException("Invalid email or password");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                    {
                        throw new UnauthorizedAccessException("Account is locked due to multiple failed login attempts. Please try again later.");
                    }
                    throw new UnauthorizedAccessException("Invalid email or password");
                }

                if (!user.IsActive)
                {
                    throw new UnauthorizedAccessException("Account is deactivated. Please contact administrator.");
                }

                // Get user roles from Identity system
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles == null || !userRoles.Any())
                {
                    throw new UnauthorizedAccessException("User has no assigned role. Please contact administrator.");
                }

                // Get the first role (assuming single role per user, modify if multiple roles needed)
                var roleName = userRoles.First();
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    throw new UnauthorizedAccessException("User role not found. Please contact administrator.");
                }

                // Map user to UserDto and include role information
                var userDto = _mapper.Map<UserDto>(user);
                userDto.UserId = user.Id;
                userDto.RoleId = role.Id;
                userDto.RoleName = role.Name ?? string.Empty;

                var token = GenerateJwtToken(userDto);

                return new LoginResponseDto
                {
                    Token = token,
                    User = userDto
                };
            }
            catch (UnauthorizedAccessException)
            {
                // Re-throw authorization exceptions as-is
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to process login request. Please check your connection and try again.", ex);
            }
        }

        public async Task<UserDto> RegisterAsync(CreateUserDto createUserDto)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(createUserDto.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("An account with this email address already exists.");
                }

                // Validate role exists
                var role = await _roleManager.FindByIdAsync(createUserDto.RoleId);
                if (role == null)
                {
                    throw new InvalidOperationException("Invalid role specified. Please select a valid role.");
                }

                // Create ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = createUserDto.Email,
                    Email = createUserDto.Email,
                    FullName = createUserDto.FullName,
                    Phone = createUserDto.Phone,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, createUserDto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Registration failed: {errors}");
                }

                // Assign role
                await _userManager.AddToRoleAsync(user, role.Name!);

                // Create UserDto for response
                var userDto = _mapper.Map<UserDto>(user);
                userDto.UserId = user.Id;
                userDto.RoleId = role.Id;
                userDto.RoleName = role.Name ?? string.Empty;

                // If role is Inspector, create Inspector record
                if (RoleConstants.IsInspectorRole(role.Name))
                {
                    var inspector = new Inspector
                    {
                        UserId = user.Id,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.Inspectors.AddAsync(inspector);
                    await _unitOfWork.SaveChangesAsync();
                }
                 await _unitOfWork.SaveChangesAsync();
                return userDto;
            }
            catch (InvalidOperationException)
            {
                // Re-throw business logic exceptions as-is
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to complete registration. Please check your connection and try again.", ex);
            }
        }

        public string GenerateJwtToken(UserDto user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "DefaultSecretKeyForDevelopment123456789");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("RoleId", user.RoleId)
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
