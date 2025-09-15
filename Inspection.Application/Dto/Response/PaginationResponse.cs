namespace Inspection.Application.Response.Dto
{
    public class PaginationResponse<T>
    {
        public List<T> Values { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
    }
}
