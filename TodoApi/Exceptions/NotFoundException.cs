namespace TodoApi.Exceptions;
public class NotFoundException(string errorMessage) : Exception(errorMessage)
{
}