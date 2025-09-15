using MediatR;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.Entities.Queries
{
    public class GetCategoriesQuery : IRequest<IEnumerable<EntityCategory>>
    {
    }
}
