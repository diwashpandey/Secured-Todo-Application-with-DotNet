namespace TodoApi.Exceptions;
public class BadRequestException(string errorMessage) : Exception(errorMessage)
{
    public int StatusCode {get;} = 404;
}