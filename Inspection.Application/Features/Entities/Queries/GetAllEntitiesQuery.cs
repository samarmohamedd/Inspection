using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.Entities.Queries
{
    public record GetAllEntitiesQuery : IRequest<IEnumerable<EntityToInspectDto>>
    {
    }
}
