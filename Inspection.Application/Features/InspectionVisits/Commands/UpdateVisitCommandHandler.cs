using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.InspectionVisits.Commands
{
    public class UpdateVisitCommandHandler : IRequestHandler<UpdateVisitCommand, InspectionVisitDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateVisitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InspectionVisitDto?> Handle(UpdateVisitCommand request, CancellationToken cancellationToken)
        {
            var visit = await _unitOfWork.InspectionVisits.GetByIdAsync(request.Id);
            if (visit == null)
            {
                return null;
            }

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

            // Check if visit is already completed
            if (visit.Status == InspectionStatus.Completed)
            {
                throw new InvalidOperationException("Cannot update a completed inspection visit");
            }

            visit.EntityToInspectId = request.EntityToInspectId;
            visit.InspectorId = request.InspectorId;
            visit.ScheduledAt = request.ScheduledAt;
            visit.Status = request.Status;
            visit.Score = request.Score;
            visit.Notes = request.Notes;
            visit.UpdatedAt = DateTime.UtcNow;

            if (request.Status == InspectionStatus.Completed && visit.CompletedAt == null)
            {
                visit.CompletedAt = DateTime.UtcNow;
            }

            _unitOfWork.InspectionVisits.Update(visit);
            await _unitOfWork.SaveChangesAsync();

            // Reload with navigation properties
            var updatedVisit = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .Include(v => v.EntityToInspect)
                .Include(v => v.Inspector)
                .Include(v => v.Violations)
                .FirstAsync(v => v.Id == visit.Id, cancellationToken);

            return _mapper.Map<InspectionVisitDto>(updatedVisit);
        }
    }
}
