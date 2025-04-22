using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs.ApiResponse;

namespace TodoApi.Common;

public class CustomControllerBase : ControllerBase
{
    protected string? GetUserId() => User.FindFirst("UserId")?.Value;
    protected string? GetUserUsername() => User.FindFirst(ClaimTypes.Name)?.Value;

}