using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.InspectionVisits.Queries
{
    public class GetDashboardQuery : IRequest<DashboardDto>
    {
    }
}
