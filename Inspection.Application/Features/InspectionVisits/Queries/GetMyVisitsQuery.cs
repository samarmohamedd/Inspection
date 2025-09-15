using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.InspectionVisits.Queries
{
    public class GetMyVisitsQuery : IRequest<IEnumerable<InspectionVisitDto>>
    {
        public int InspectorId { get; set; }

        public GetMyVisitsQuery(int inspectorId)
        {
            InspectorId = inspectorId;
        }
    }
}
