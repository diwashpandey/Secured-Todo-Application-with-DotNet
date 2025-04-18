using TodoApi.Models;

namespace TodoApi.DTOs;

public class GetTodosResponse
{
    public List<Todo> Todos {get; set;} = [];
}