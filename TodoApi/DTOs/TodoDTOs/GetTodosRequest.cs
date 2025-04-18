using TodoApi.Models;

namespace TodoApi.DTOs.TodoDTOs;

public class GetTodosResponse
{
    public List<Todo> Todos {get; set;} = [];
}