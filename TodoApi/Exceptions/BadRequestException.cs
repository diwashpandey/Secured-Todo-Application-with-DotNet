namespace TodoApi.Exceptions;
public class BadRequestException(string errorMessage) : Exception(errorMessage)
{
    public static int StatusCode {get;} = 400;
}