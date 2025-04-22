namespace TodoApi.DTOs.TodoDTOs;
public class UpdateTodoRequest
{
    public string Id {get; set;} = string.Empty;
    public string Field {get; set;} = string.Empty;
    public string Data {get; set;} = string.Empty;
}