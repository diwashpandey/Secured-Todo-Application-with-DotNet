using Microsoft.AspNetCore.Http.HttpResults;
using TodoApi.DTOs.ApiResponse;
using TodoApi.Exceptions;

namespace TodoApi.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Dictionary<Type, Action<HttpContext>> _exceptionsMethods = new (){
        {
            typeof(NotFoundException),
            context => {
                context.Response.StatusCode = NotFoundException.StatusCode;
            }
        },
        {
            typeof(UnauthorizedException),
            context => {
                context.Response.StatusCode = UnauthorizedException.StatusCode;
            }
        },
        {
            typeof(BadRequestException),
            context => {
                context.Response.StatusCode = BadRequestException.StatusCode;
            }
        },
    };


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
        if (_exceptionsMethods.TryGetValue(exception.GetType(), out var handler))
        {
            handler(context);
        }
        else context.Response.StatusCode = 500;

        await context.Response.WriteAsJsonAsync (new ApiResponse {
            MessageFromServer = exception.Message,
            TraceId = context.TraceIdentifier
        });
    }


    
    // private readonly List<Type> _exceptions =[
    //         typeof(NotFoundException),
    //         typeof(BadRequestException),
    //         typeof(UnauthorizedException)
    //     ];

    // private async Task HandleException(Exception exception, HttpContext context)
    // {
    //     if (_exceptions.Contains(exception.GetType()))
    //     {
    //         Console.WriteLine("IN custom exception");
    //         // handle custom exception
    //         if (exception is NotFoundException _notFoundException)
    //             context.Response.StatusCode = _notFoundException.StatusCode;

    //         else if (exception is UnauthorizedException _unauthorizedException)
    //             context.Response.StatusCode = _unauthorizedException.StatusCode;
            
    //         else if(exception is BadRequestException _badRequestException)
    //             context.Response.StatusCode = _badRequestException.StatusCode;
    //     }
    //     else context.Response.StatusCode = 500;

    //     await context.Response.WriteAsJsonAsync (new ApiResponse {
    //         MessageFromServer = exception.Message,
    //         TraceId = context.TraceIdentifier
    //     });
    // }
}