namespace TodoApi.Exceptions;
public class UnauthorizedException(string errorMessage) : Exception(errorMessage)
{
    public int StatusCode {get;} = 401;
}