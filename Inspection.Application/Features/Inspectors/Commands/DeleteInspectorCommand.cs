using MediatR;

namespace Inspection.Application.Features.Inspectors.Commands
{
    public record DeleteInspectorCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteInspectorCommand(int id)
        {
            Id = id;
        }
    }
}
