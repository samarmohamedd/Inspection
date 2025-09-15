using MediatR;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;

namespace Inspection.Application.Features.Entities.Queries
{
    public class GetAllEntitiesQueryHandler : IRequestHandler<GetAllEntitiesQuery, IEnumerable<EntityToInspectDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEntitiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EntityToInspectDto>> Handle(GetAllEntitiesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.EntitiesToInspect.GetAllAsync();
            return _mapper.Map<IEnumerable<EntityToInspectDto>>(entities);
        }
    }
}
