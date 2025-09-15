using MediatR;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.InspectionVisits.Commands
{
    public class DeleteVisitCommandHandler : IRequestHandler<DeleteVisitCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteVisitCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteVisitCommand request, CancellationToken cancellationToken)
        {
            var visit = await _unitOfWork.InspectionVisits.GetByIdAsync(request.Id);
            if (visit == null)
            {
                return false;
            }

            if (visit.Status == InspectionStatus.Completed)
            {
                throw new InvalidOperationException("Cannot delete a completed inspection visit");
            }

            await _unitOfWork.InspectionVisits.DeleteAsync(visit);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
