using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.Application.Abstractions;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Entities;

namespace Inspection.Application.Features.Auth.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, InspectorDto>
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

        public async Task<InspectorDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingInspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
            
            if (existingInspector != null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            var inspector = new Inspector
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                Role = request.Role,
                PasswordHash = _authService.HashPassword(request.Password),
                IsActive = true
            };

            await _unitOfWork.Inspectors.AddAsync(inspector);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<InspectorDto>(inspector);
        }
    }
}
