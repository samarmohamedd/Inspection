using MediatR;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;

namespace Inspection.Application.Features.Entities.Queries
{
    public class GetEntityByIdQueryHandler : IRequestHandler<GetEntityByIdQuery, EntityToInspectDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEntityByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EntityToInspectDto?> Handle(GetEntityByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.EntitiesToInspect.GetByIdAsync(request.Id);
            return entity != null ? _mapper.Map<EntityToInspectDto>(entity) : null;
        }
    }
}
