namespace Common.Models;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = [];
    public string CorrelationId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> Success(T data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Error(string message, string? details = null) 
    {
        var response =  new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
        };

        if (!string.IsNullOrEmpty(details))
               response.Errors.Add(details);

        return response;
    }

    public static ApiResponse<T> Error(List<string> errors, string message = "Validation failed")
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors
        };
    }
}