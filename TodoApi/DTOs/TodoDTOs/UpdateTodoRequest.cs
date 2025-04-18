namespace TodoApi.DTOs.TodoDTOs;
public class UpdateTodoRequest
{
    public string id {get; set;} = string.Empty;
    public string field {get; set;} = string.Empty;
    public string data {get; set;} = string.Empty;
}