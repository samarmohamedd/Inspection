using MediatR;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Enum;
using System.Linq;

namespace Inspection.Application.Features.Entities.Queries
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<EntityCategory>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<EntityCategory>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            // Return all enum values
            var categories = Enum.GetValues<EntityCategory>().OrderBy(c => c).AsEnumerable();
            return Task.FromResult(categories);
        }
    }
}
