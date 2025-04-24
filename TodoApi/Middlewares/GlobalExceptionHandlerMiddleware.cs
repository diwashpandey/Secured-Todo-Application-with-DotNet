using TodoApi.DTOs.ApiResponse;
using TodoApi.Exceptions;

namespace TodoApi.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    private readonly List<Type> _exceptions =[typeof(NotFoundException),typeof(UnauthorizedException)];

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(ex, context);
        }
    }

    private async Task HandleException(Exception exception, HttpContext context)
    {
        if (_exceptions.Contains(exception.GetType()))
        {
            // handle custom exception
            if (exception is NotFoundException _notFoundException)
                context.Response.StatusCode = _notFoundException.StatusCode;

            if (exception is UnauthorizedException _unauthorizedException)
                context.Response.StatusCode = _unauthorizedException.StatusCode;
        }
        else context.Response.StatusCode = 500;

        await context.Response.WriteAsJsonAsync (new ApiResponse {
            MessageFromServer = exception.Message,
            TraceId = context.TraceIdentifier
        });
    }
}