using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.InspectionVisits.Queries
{
    public record GetMyVisitsQuery : IRequest<IEnumerable<InspectionVisitDto>>
    {
        public string UserId { get; set; }

        public GetMyVisitsQuery(string userId)
        {
            UserId = userId;
        }
    }
}
