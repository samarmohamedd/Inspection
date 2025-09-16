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
            var loginDto = new LoginDto
            {
                Email = request.Email,
                Password = request.Password
            };

            return await _authService.LoginAsync(loginDto);
        }
    }
}
