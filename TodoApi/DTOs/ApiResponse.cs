namespace TodoApi.DTOs.ApiResponse;

/// <summary>
/// A generic response wrapper for API results.
/// </summary>
/// <typeparam name="TData">The type of data being returned, e.g., Todo, User, or any object.</typeparam>
public class ApiResponse
{
    public bool SuccessStatus {get; set;} = false;
    public string? MessageFromServer {get; set;}
}

public class ApiResponse<TData> : ApiResponse
{
    public TData? Data {get; set;}
}
