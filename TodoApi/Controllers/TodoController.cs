using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TodoApi.Models;
using TodoApi.DTOs;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly TodoService _todoService;

    public TodoController(TodoService todoService){
        _todoService = todoService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Todo>> GetTodos()
    {
        List<Todo> todos = _todoService.FindAllTodos();
        return todos;
    }

    [HttpPost]
    public IActionResult PostTodo([FromBody] Todo todo)
    {
        bool success = _todoService.InsertTodo(todo);
        return success ? Ok("Todo inserted successfully!") : BadRequest("Error inserting Todo!");
    }

    [HttpDelete]
    public ActionResult<string> RemoveTodo([FromBody] DeleteTodoRequest req){
        bool success = _todoService.DeleteTodo(req.id);
        return success ? Ok("Todo Deleted!") : BadRequest("Error occured!");
    }

    [HttpPut]
    public ActionResult<string> PutTodo([FromBody] Todo new_todo){
        bool success = _todoService.UpdateFullTodo(new_todo);
        return success ? Ok("Todo Fully Updated!") : BadRequest("Error occured!");
    }

    [HttpPatch]
    public ActionResult<string> UpdateTodo([FromBody] UpdateTodoRequest data){
        bool success = _todoService.UpdateField(data.id, data.field, data.data);
        return success ? Ok("Filed updated!") : BadRequest("Error occured!");
    }
}
