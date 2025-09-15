using MediatR;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;

namespace Inspection.Application.Features.Entities.Commands
{
    public class UpdateEntityCommandHandler : IRequestHandler<UpdateEntityCommand, EntityToInspectDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateEntityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EntityToInspectDto?> Handle(UpdateEntityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.EntitiesToInspect.GetByIdAsync(request.Id);
            if (entity == null)
            {
                return null;
            }

            entity.Name = request.Name;
            entity.Address = request.Address;
            entity.Category = request.Category;
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.EntitiesToInspect.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EntityToInspectDto>(entity);
        }
    }
}
