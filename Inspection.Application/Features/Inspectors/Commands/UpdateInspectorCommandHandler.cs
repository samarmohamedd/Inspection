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
            var inspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (inspector == null)
            {
                return null;
            }

            // For now, only update Inspector-specific properties
            // User properties should be updated through Identity UserManager
            inspector.IsActive = request.IsActive;
            inspector.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Inspectors.Update(inspector);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<InspectorDto>(inspector);
        }
    }
}
