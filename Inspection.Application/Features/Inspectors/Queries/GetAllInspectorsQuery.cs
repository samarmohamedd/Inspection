using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.Inspectors.Queries
{
    public class GetAllInspectorsQuery : IRequest<IEnumerable<InspectorDto>>
    {
    }
}
