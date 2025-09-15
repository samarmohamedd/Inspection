using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.InspectionVisits.Queries
{
    public class GetVisitByIdQuery : IRequest<InspectionVisitDto?>
    {
        public int Id { get; set; }

        public GetVisitByIdQuery(int id)
        {
            Id = id;
        }
    }
}
