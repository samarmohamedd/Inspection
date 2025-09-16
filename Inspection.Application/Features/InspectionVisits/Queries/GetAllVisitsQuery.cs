using MediatR;
using Inspection.Application.Dto;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.InspectionVisits.Queries
{
    public record GetAllVisitsQuery : IRequest<PagedResultDto<InspectionVisitDto>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public InspectionStatus? Status { get; set; }
        public int? InspectorId { get; set; }
        public EntityCategory? Category { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetAllVisitsQuery(InspectionVisitFilterDto filter)
        {
            StartDate = filter.StartDate;
            EndDate = filter.EndDate;
            Status = filter.Status;
            InspectorId = filter.InspectorId;
            Category = filter.Category;
            PageNumber = filter.PageNumber;
            PageSize = filter.PageSize;
        }
    }
}
