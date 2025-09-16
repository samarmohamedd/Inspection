using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.InspectionVisits.Queries
{
    public record GetDashboardQuery : IRequest<DashboardDto>
    {
    }
}
