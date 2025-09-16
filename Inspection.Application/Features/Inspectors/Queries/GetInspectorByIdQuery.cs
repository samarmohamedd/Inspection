using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.Inspectors.Queries
{
    public record GetInspectorByIdQuery : IRequest<InspectorDto?>
    {
        public int Id { get; set; }

        public GetInspectorByIdQuery(int id)
        {
            Id = id;
        }
    }
}
