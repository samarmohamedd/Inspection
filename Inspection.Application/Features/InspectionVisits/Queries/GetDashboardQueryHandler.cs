using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.InspectionVisits.Queries
{
    public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDashboardQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;

            // Get all visits for current month
            var visitsThisMonth = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .Where(v => v.CreatedAt.Month == currentMonth && v.CreatedAt.Year == currentYear)
                .ToListAsync(cancellationToken);

            // Get counts by status for all visits (not just this month)
            var allVisits = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .ToListAsync(cancellationToken);

            var visitCountsByStatus = allVisits
                .GroupBy(v => v.Status)
                .ToDictionary(g => g.Key, g => g.Count());

            // Ensure all statuses are represented
            foreach (InspectionStatus status in Enum.GetValues<InspectionStatus>())
            {
                if (!visitCountsByStatus.ContainsKey(status))
                {
                    visitCountsByStatus[status] = 0;
                }
            }

            // Calculate average score for completed visits this month
            var completedVisitsThisMonth = visitsThisMonth
                .Where(v => v.Status == InspectionStatus.Completed && v.Score.HasValue)
                .ToList();

            var averageScoreThisMonth = completedVisitsThisMonth.Any() 
                ? completedVisitsThisMonth.Average(v => v.Score!.Value) 
                : 0;

            // Get total entities inspected (entities that have at least one visit)
            var totalEntitiesInspected = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .Select(v => v.EntityToInspectId)
                .Distinct()
                .CountAsync(cancellationToken);

            // Get total active inspectors
            var totalActiveInspectors = await _unitOfWork.Inspectors.GetAsQueryable()
                .Where(i => i.IsActive)
                .CountAsync(cancellationToken);

            // Get recent visits (last 10)
            var recentVisits = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .Include(v => v.EntityToInspect)
                .Include(v => v.Inspector)
                .OrderByDescending(v => v.CreatedAt)
                .Take(10)
                .Select(v => new RecentVisitDto
                {
                    Id = v.Id,
                    EntityName = v.EntityToInspect.Name,
                    InspectorName = v.Inspector.FullName,
                    ScheduledAt = v.ScheduledAt,
                    Status = v.Status,
                    Score = v.Score
                })
                .ToListAsync(cancellationToken);

            return new DashboardDto
            {
                VisitCountsByStatus = visitCountsByStatus,
                AverageScoreThisMonth = Math.Round(averageScoreThisMonth, 2),
                TotalVisitsThisMonth = visitsThisMonth.Count,
                TotalEntitiesInspected = totalEntitiesInspected,
                TotalActiveInspectors = totalActiveInspectors,
                RecentVisits = recentVisits
            };
        }
    }
}
