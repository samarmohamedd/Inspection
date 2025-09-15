using MediatR;

namespace Inspection.Application.Features.Entities.Commands
{
    public class DeleteEntityCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteEntityCommand(int id)
        {
            Id = id;
        }
    }
}
