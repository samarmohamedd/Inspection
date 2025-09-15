using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Entities;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.InspectionVisits.Commands
{
    public class CreateVisitCommandHandler : IRequestHandler<CreateVisitCommand, InspectionVisitDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateVisitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InspectionVisitDto> Handle(CreateVisitCommand request, CancellationToken cancellationToken)
        {
            // Validate entity exists
            var entityExists = await _unitOfWork.EntitiesToInspect.ExistsAsync(e => e.Id == request.EntityToInspectId);
            if (!entityExists)
            {
                throw new InvalidOperationException("Entity to inspect not found");
            }

            // Validate inspector exists
            var inspectorExists = await _unitOfWork.Inspectors.ExistsAsync(i => i.Id == request.InspectorId);
            if (!inspectorExists)
            {
                throw new InvalidOperationException("Inspector not found");
            }

            // Check for conflicting visits
            var conflictingVisit = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .AnyAsync(v => v.InspectorId == request.InspectorId && 
                              v.ScheduledAt.Date == request.ScheduledAt.Date &&
                              v.Status != InspectionStatus.Completed &&
                              v.Status != InspectionStatus.Cancelled, cancellationToken);

            if (conflictingVisit)
            {
                throw new InvalidOperationException("Inspector already has a visit scheduled for this date");
            }

            var visit = new InspectionVisit
            {
                EntityToInspectId = request.EntityToInspectId,
                InspectorId = request.InspectorId,
                ScheduledAt = request.ScheduledAt,
                Notes = request.Notes,
                Status = InspectionStatus.Planned
            };

            await _unitOfWork.InspectionVisits.AddAsync(visit);
            await _unitOfWork.SaveChangesAsync();

            // Reload with navigation properties
            var createdVisit = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .Include(v => v.EntityToInspect)
                .Include(v => v.Inspector)
                .Include(v => v.Violations)
                .FirstAsync(v => v.Id == visit.Id, cancellationToken);

            return _mapper.Map<InspectionVisitDto>(createdVisit);
        }
    }
}
