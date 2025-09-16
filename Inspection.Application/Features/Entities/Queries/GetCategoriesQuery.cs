using MediatR;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.Entities.Queries
{
    public record GetCategoriesQuery : IRequest<IEnumerable<EntityCategory>>
    {
    }
}
