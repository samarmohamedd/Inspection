using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;

namespace Inspection.Application.Features.InspectionVisits.Queries
{
    public class GetMyVisitsQueryHandler : IRequestHandler<GetMyVisitsQuery, IEnumerable<InspectionVisitDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMyVisitsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InspectionVisitDto>> Handle(GetMyVisitsQuery request, CancellationToken cancellationToken)
        {
            var visits = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .Include(v => v.EntityToInspect)
                .Include(v => v.Inspector)
                .Include(v => v.Violations)
                .Where(v => v.InspectorId == request.InspectorId)
                .OrderByDescending(v => v.ScheduledAt)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<InspectionVisitDto>>(visits);
        }
    }
}
