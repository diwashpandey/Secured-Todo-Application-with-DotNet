namespace TodoApi.Exceptions;
public class NotFoundException(string errorMessage) : Exception(errorMessage)
{
    public int StatusCode {get;} = 404;
}