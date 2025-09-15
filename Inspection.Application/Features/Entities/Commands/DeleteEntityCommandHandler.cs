using MediatR;
using Inspection.DataAccessLayer.Repository;

namespace Inspection.Application.Features.Entities.Commands
{
    public class DeleteEntityCommandHandler : IRequestHandler<DeleteEntityCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEntityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteEntityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.EntitiesToInspect.GetByIdAsync(request.Id);
            if (entity == null)
            {
                return false;
            }

            await _unitOfWork.EntitiesToInspect.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
