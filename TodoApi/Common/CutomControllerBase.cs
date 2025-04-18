using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Common;

public class CustomControllerBase : ControllerBase
{
    protected string? GetUserId() => User.FindFirst("UserId")?.Value;


}