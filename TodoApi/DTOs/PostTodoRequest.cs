namespace TodoApi.DTOs;

public class PostTodoRequest
{
    public string Description { get; set; } = string.Empty;
    public DateTime? DeadLine {get; set;}
}