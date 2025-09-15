using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;

namespace Inspection.Application.Features.Inspectors.Commands
{
    public class UpdateInspectorCommandHandler : IRequestHandler<UpdateInspectorCommand, InspectorDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateInspectorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InspectorDto?> Handle(UpdateInspectorCommand request, CancellationToken cancellationToken)
        {
            var inspector = await _unitOfWork.Inspectors.GetByIdAsync(request.Id);
            if (inspector == null)
            {
                return null;
            }

            // Check if email is being changed and if it already exists
            if (inspector.Email != request.Email)
            {
                var emailExists = await _unitOfWork.Inspectors.GetAsQueryable()
                    .AnyAsync(x => x.Email == request.Email && x.Id != request.Id, cancellationToken);
                
                if (emailExists)
                {
                    throw new InvalidOperationException("Email already exists");
                }
            }

            inspector.FullName = request.FullName;
            inspector.Email = request.Email;
            inspector.Phone = request.Phone;
            inspector.Role = request.Role;
            inspector.IsActive = request.IsActive;
            inspector.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Inspectors.Update(inspector);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<InspectorDto>(inspector);
        }
    }
}
