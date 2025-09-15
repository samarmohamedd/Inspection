using MediatR;

namespace Inspection.Application.Features.InspectionVisits.Commands
{
    public class DeleteVisitCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteVisitCommand(int id)
        {
            Id = id;
        }
    }
}
