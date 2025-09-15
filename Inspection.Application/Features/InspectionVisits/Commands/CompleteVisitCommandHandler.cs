using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Entities;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.InspectionVisits.Commands
{
    public class CompleteVisitCommandHandler : IRequestHandler<CompleteVisitCommand, InspectionVisitDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompleteVisitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InspectionVisitDto?> Handle(CompleteVisitCommand request, CancellationToken cancellationToken)
        {
            var visit = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .Include(v => v.Violations)
                .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

            if (visit == null)
            {
                return null;
            }

            if (visit.Status == InspectionStatus.Completed)
            {
                throw new InvalidOperationException("Inspection visit is already completed");
            }

            if (visit.Status != InspectionStatus.InProgress)
            {
                throw new InvalidOperationException("Only in-progress visits can be completed");
            }

            // Update visit
            visit.Status = InspectionStatus.Completed;
            visit.Score = request.Score;
            visit.Notes = request.Notes;
            visit.CompletedAt = DateTime.UtcNow;
            visit.UpdatedAt = DateTime.UtcNow;

            // Add violations
            foreach (var violationDto in request.Violations)
            {
                var violation = new Violation
                {
                    InspectionVisitId = visit.Id,
                    Code = violationDto.Code,
                    Description = violationDto.Description,
                    Severity = violationDto.Severity
                };
                await _unitOfWork.Violations.AddAsync(violation);
            }

            _unitOfWork.InspectionVisits.Update(visit);
            await _unitOfWork.SaveChangesAsync();

            // Reload with all navigation properties
            var completedVisit = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .Include(v => v.EntityToInspect)
                .Include(v => v.Inspector)
                .Include(v => v.Violations)
                .FirstAsync(v => v.Id == visit.Id, cancellationToken);

            return _mapper.Map<InspectionVisitDto>(completedVisit);
        }
    }
}
