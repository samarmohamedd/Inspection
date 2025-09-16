using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Inspection.Application.Dto;
using Inspection.Application.Features.Auth.Commands;
using Inspection.Domain.Entities;

namespace Inspection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AuthController(IMediator mediator, ILogger<AuthController> logger, RoleManager<ApplicationRole> roleManager)
        {
            _mediator = mediator;
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var command = new LoginCommand(loginDto);
                var result = await _mediator.Send(command);

                _logger.LogInformation("User {Email} logged in successfully",
                    loginDto.Email);

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Login failed for {Email}: {Message}", loginDto.Email, ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Login failed for {Email}: {Message}", loginDto.Email, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for {Email} from IP {ClientIP}",
                    loginDto.Email, HttpContext.Connection.RemoteIpAddress?.ToString());
                return StatusCode(500, new { message = "An unexpected error occurred during login. Please try again later." });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<InspectorDto>> Register([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var command = new RegisterCommand(createUserDto);
                var result = await _mediator.Send(command);

                          return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Registration failed for {Email}: {Message}", createUserDto.Email, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Registration failed for {Email}: {Message}", createUserDto.Email, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for {Email} with role {RoleId}",
                    createUserDto.Email, createUserDto.RoleId);
                return StatusCode(500, new { message = "An unexpected error occurred during registration. Please try again later." });
            }
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<object>>> GetRoles()
        {
            try
            {
                var roles = _roleManager.Roles
                    .Select(r => new { id = r.Id, name = r.Name, description = r.Description })
                    .ToList();

                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving roles");
                return StatusCode(500, new { message = "An error occurred while retrieving roles" });
            }
        }
    }
}
