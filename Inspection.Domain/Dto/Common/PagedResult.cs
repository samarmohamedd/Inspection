namespace Inspection.Domain.Dto.Common;

/// <summary>
/// Represents a paged result for list operations
/// </summary>
/// <typeparam name="T">Type of items in the result</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// List of items for the current page
    /// </summary>
    public List<T> Items { get; set; } = new List<T>();
    
    /// <summary>
    /// Total number of items across all pages
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// Current page number (1-based)
    /// </summary>
    public int PageNumber { get; set; }
    
    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize { get; set; }
    
    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    
    /// <summary>
    /// Indicates whether there is a previous page
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;
    
    /// <summary>
    /// Indicates whether there is a next page
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;
    
    /// <summary>
    /// Creates a new paged result
    /// </summary>
    /// <param name="items">Items for the current page</param>
    /// <param name="totalCount">Total number of items</param>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Number of items per page</param>
    public PagedResult(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    
    /// <summary>
    /// Creates an empty paged result
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Number of items per page</param>
    public PagedResult(int pageNumber, int pageSize)
    {
        Items = new List<T>();
        TotalCount = 0;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
