using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;

namespace Inspection.Application.Features.InspectionVisits.Queries
{
    public class GetVisitByIdQueryHandler : IRequestHandler<GetVisitByIdQuery, InspectionVisitDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetVisitByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InspectionVisitDto?> Handle(GetVisitByIdQuery request, CancellationToken cancellationToken)
        {
            var visit = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .Include(v => v.EntityToInspect)
                .Include(v => v.Inspector)
                .Include(v => v.Violations)
                .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

            return visit != null ? _mapper.Map<InspectionVisitDto>(visit) : null;
        }
    }
}
