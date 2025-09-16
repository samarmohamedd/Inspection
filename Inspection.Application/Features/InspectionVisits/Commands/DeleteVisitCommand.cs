using MediatR;

namespace Inspection.Application.Features.InspectionVisits.Commands
{
    public record DeleteVisitCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteVisitCommand(int id)
        {
            Id = id;
        }
    }
}
