namespace Inspection.Domain.Dto.Common;

/// <summary>
/// Generic API response wrapper
/// </summary>
/// <typeparam name="T">Type of data being returned</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates whether the operation was successful
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// The data returned by the operation
    /// </summary>
    public T? Data { get; set; }
    
    /// <summary>
    /// Error message if the operation failed
    /// </summary>
    public string? Message { get; set; }
    
    /// <summary>
    /// List of validation errors
    /// </summary>
    public List<string>? Errors { get; set; }
    
    /// <summary>
    /// Creates a successful response with data
    /// </summary>
    /// <param name="data">The data to return</param>
    /// <returns>Successful API response</returns>
    public static ApiResponse<T> SuccessResult(T data)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data
        };
    }
    
    /// <summary>
    /// Creates a successful response with data and message
    /// </summary>
    /// <param name="data">The data to return</param>
    /// <param name="message">Success message</param>
    /// <returns>Successful API response</returns>
    public static ApiResponse<T> SuccessResult(T data, string message)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }
    
    /// <summary>
    /// Creates a failure response with error message
    /// </summary>
    /// <param name="message">Error message</param>
    /// <returns>Failed API response</returns>
    public static ApiResponse<T> FailureResult(string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message
        };
    }
    
    /// <summary>
    /// Creates a failure response with validation errors
    /// </summary>
    /// <param name="errors">List of validation errors</param>
    /// <returns>Failed API response</returns>
    public static ApiResponse<T> FailureResult(List<string> errors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Errors = errors
        };
    }
}

/// <summary>
/// API response without data
/// </summary>
public class ApiResponse : ApiResponse<object>
{
    /// <summary>
    /// Creates a successful response without data
    /// </summary>
    /// <returns>Successful API response</returns>
    public static ApiResponse SuccessResult()
    {
        return new ApiResponse
        {
            Success = true
        };
    }
    
    /// <summary>
    /// Creates a successful response with message
    /// </summary>
    /// <param name="message">Success message</param>
    /// <returns>Successful API response</returns>
    public static ApiResponse SuccessResult(string message)
    {
        return new ApiResponse
        {
            Success = true,
            Message = message
        };
    }
}
