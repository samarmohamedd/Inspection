using MediatR;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;

namespace Inspection.Application.Features.Inspectors.Queries
{
    public class GetAllInspectorsQueryHandler : IRequestHandler<GetAllInspectorsQuery, IEnumerable<InspectorDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllInspectorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InspectorDto>> Handle(GetAllInspectorsQuery request, CancellationToken cancellationToken)
        {
            var inspectors = await _unitOfWork.Inspectors.GetAllAsync();
            return _mapper.Map<IEnumerable<InspectorDto>>(inspectors);
        }
    }
}
