using Inspection.Domain.Enum;

namespace Inspection.Application.Dto
{
    public class DashboardDto
    {
        public Dictionary<InspectionStatus, int> VisitCountsByStatus { get; set; } = new Dictionary<InspectionStatus, int>();
        public double AverageScoreThisMonth { get; set; }
        public int TotalVisitsThisMonth { get; set; }
        public int TotalEntitiesInspected { get; set; }
        public int TotalActiveInspectors { get; set; }
        public List<RecentVisitDto> RecentVisits { get; set; } = new List<RecentVisitDto>();
    }

    public class RecentVisitDto
    {
        public int Id { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string InspectorName { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public InspectionStatus Status { get; set; }
        public int? Score { get; set; }
    }

    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
