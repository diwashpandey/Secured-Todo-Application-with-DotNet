namespace TodoApi.Exceptions;
public class UnauthorizedException(string errorMessage) : Exception(errorMessage)
{
    public static int StatusCode {get;} = 401;
}