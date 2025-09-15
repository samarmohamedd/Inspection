using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.Entities.Queries
{
    public class GetEntityByIdQuery : IRequest<EntityToInspectDto?>
    {
        public int Id { get; set; }

        public GetEntityByIdQuery(int id)
        {
            Id = id;
        }
    }
}
