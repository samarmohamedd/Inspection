using MediatR;
using Microsoft.EntityFrameworkCore;
using Inspection.DataAccessLayer.Repository;

namespace Inspection.Application.Features.Inspectors.Commands
{
    public class DeleteInspectorCommandHandler : IRequestHandler<DeleteInspectorCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInspectorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteInspectorCommand request, CancellationToken cancellationToken)
        {
            var inspector = await _unitOfWork.Inspectors.GetByIdAsync(request.Id);
            if (inspector == null)
            {
                return false;
            }

            // Check if inspector has any inspection visits
            var hasVisits = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .AnyAsync(v => v.InspectorId == request.Id, cancellationToken);

            if (hasVisits)
            {
                throw new InvalidOperationException("Cannot delete inspector with existing inspection visits");
            }

            await _unitOfWork.Inspectors.DeleteAsync(inspector);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
