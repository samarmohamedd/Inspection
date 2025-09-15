using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.Application.Abstractions;
using Inspection.DataAccessLayer.Repository;

namespace Inspection.Application.Features.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public LoginCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var inspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (inspector == null || !_authService.VerifyPassword(request.Password, inspector.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            if (!inspector.IsActive)
            {
                throw new UnauthorizedAccessException("Account is deactivated");
            }

            var inspectorDto = _mapper.Map<InspectorDto>(inspector);
            var token = _authService.GenerateJwtToken(inspectorDto);

            return new LoginResponseDto
            {
                Token = token,
                Inspector = inspectorDto
            };
        }
    }
}
