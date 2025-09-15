using Microsoft.AspNetCore.Mvc;
using MediatR;
using Inspection.Application.Dto;
using Inspection.Application.Features.Auth.Commands;

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
                var command = new LoginCommand(loginDto);
                var result = await _mediator.Send(command);

                _logger.LogInformation("User {Email} logged in successfully in {ElapsedMs}ms",
                    loginDto.Email);

                return Ok(result);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Email} from IP {ClientIP} after {ElapsedMs}ms",
                    loginDto.Email, HttpContext.Connection.RemoteIpAddress?.ToString());
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<InspectorDto>> Register([FromBody] CreateInspectorDto createInspectorDto)
        {
            try
            {
                var command = new RegisterCommand(createInspectorDto);
                var result = await _mediator.Send(command);

                _logger.LogInformation("New user registered successfully: {Email} with ID {UserId} and role {Role} in {ElapsedMs}ms",
                    createInspectorDto.Email, result.Id, createInspectorDto.Role);

                return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for {Email} with role {Role} after {ElapsedMs}ms",
                    createInspectorDto.Email, createInspectorDto.Role);
                return StatusCode(500, new { message = "An error occurred during registration" });
            }
        }
    }
}
