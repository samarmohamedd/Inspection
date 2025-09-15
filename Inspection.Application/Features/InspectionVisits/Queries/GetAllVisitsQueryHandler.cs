using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;

namespace Inspection.Application.Features.InspectionVisits.Queries
{
    public class GetAllVisitsQueryHandler : IRequestHandler<GetAllVisitsQuery, PagedResultDto<InspectionVisitDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllVisitsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<InspectionVisitDto>> Handle(GetAllVisitsQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.InspectionVisits.GetAsQueryable()
                .Include(v => v.EntityToInspect)
                .Include(v => v.Inspector)
                .Include(v => v.Violations)
                .AsQueryable();

            // Apply filters
            if (request.StartDate.HasValue)
            {
                query = query.Where(v => v.ScheduledAt >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(v => v.ScheduledAt <= request.EndDate.Value);
            }

            if (request.Status.HasValue)
            {
                query = query.Where(v => v.Status == request.Status.Value);
            }

            if (request.InspectorId.HasValue)
            {
                query = query.Where(v => v.InspectorId == request.InspectorId.Value);
            }

            if (request.Category.HasValue)
            {
                query = query.Where(v => v.EntityToInspect.Category == request.Category.Value);
            }

            // Get total count
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var visits = await query
                .OrderByDescending(v => v.ScheduledAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var visitDtos = _mapper.Map<List<InspectionVisitDto>>(visits);

            return new PagedResultDto<InspectionVisitDto>
            {
                Items = visitDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
