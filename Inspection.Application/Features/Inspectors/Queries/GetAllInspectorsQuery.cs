using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.Inspectors.Queries
{
    public record GetAllInspectorsQuery : IRequest<IEnumerable<InspectorDto>>
    {
    }
}
