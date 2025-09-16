using AutoMapper;
using AutoMapper.Internal;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspection.Application.Features.Inspectors.Queries
{
    public class GetInspectorByIdQueryHandler : IRequestHandler<GetInspectorByIdQuery, InspectorDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInspectorByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InspectorDto?> Handle(GetInspectorByIdQuery request, CancellationToken cancellationToken)
        {
            var inspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .Include(a=>a.User).FirstOrDefaultAsync(a=> a.Id== request.Id);
            return inspector != null ? _mapper.Map<InspectorDto>(inspector) : null;
        }
    }
}
