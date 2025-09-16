using Microsoft.AspNetCore.Mvc;
using MediatR;
using Inspection.Application.Dto;
using Inspection.Application.Features.Auth.Commands;
using Inspection.Application.Features.Auth.Queries;

namespace Inspection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _mediator.Send(new LoginCommand(loginDto));
                return Ok(result);
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
                var result = await _mediator.Send(new RegisterCommand(createUserDto));
               return Ok(result);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for {Email} with role {RoleId}",
                    createUserDto.Email, createUserDto.RoleId);
                return StatusCode(500, new { message = "An unexpected error occurred during registration. Please try again later." });
            }
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            try
            {
                var query = new GetRolesQuery();
                var roles = await _mediator.Send(query);
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
