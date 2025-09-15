using MediatR;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Entities;

namespace Inspection.Application.Features.Entities.Commands
{
    public class CreateEntityCommandHandler : IRequestHandler<CreateEntityCommand, EntityToInspectDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEntityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EntityToInspectDto> Handle(CreateEntityCommand request, CancellationToken cancellationToken)
        {
            var entity = new EntityToInspect
            {
                Name = request.Name,
                Address = request.Address,
                Category = request.Category,
                IsActive = true
            };

            await _unitOfWork.EntitiesToInspect.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EntityToInspectDto>(entity);
        }
    }
}
