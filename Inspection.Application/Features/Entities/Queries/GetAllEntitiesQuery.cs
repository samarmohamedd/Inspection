using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.Entities.Queries
{
    public class GetAllEntitiesQuery : IRequest<IEnumerable<EntityToInspectDto>>
    {
    }
}
