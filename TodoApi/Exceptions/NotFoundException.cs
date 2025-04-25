namespace TodoApi.Exceptions;
public class NotFoundException(string errorMessage) : Exception(errorMessage)
{
    public static int StatusCode {get;} = 404;
}