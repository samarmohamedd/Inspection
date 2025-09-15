using MediatR;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;

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
            var inspector = await _unitOfWork.Inspectors.GetByIdAsync(request.Id);
            return inspector != null ? _mapper.Map<InspectorDto>(inspector) : null;
        }
    }
}
