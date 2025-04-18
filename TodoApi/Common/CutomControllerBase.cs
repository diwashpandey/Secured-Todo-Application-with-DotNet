using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs.ApiResponse;

namespace TodoApi.Common;

public class CustomControllerBase : ControllerBase
{
    protected string? GetUserId() => User.FindFirst("UserId")?.Value;

    protected ApiResponse<TData> GenerateResponse<TData>(bool successStatus, string? messageFromServer, TData data)
    {
    /// Generates a standardized API response structure.
        return new ApiResponse<TData>()
        {
            SuccessStatus = successStatus,
            MessageFromServer = messageFromServer,
            Data = data
        };
    }
}