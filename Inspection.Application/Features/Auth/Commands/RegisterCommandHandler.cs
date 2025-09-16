using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.Application.Abstractions;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Entities;

namespace Inspection.Application.Features.Auth.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var createUserDto = new CreateUserDto
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                RoleId = request.RoleId,
                Password = request.Password
            };

            return await _authService.RegisterAsync(createUserDto);
        }
    }
}
