using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TodoApi.Models;
using TodoApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly TodoService _todoService;

    public TodoController(TodoService todoService){
        _todoService = todoService;
    }

    [Authorize]
    [HttpGet]
    public async Task<GetTodosResponse> GetTodosByUser()
    {
        string? userId = User.FindFirst("UserId")?.Value;
        return await _todoService.GetTodosByUserAsync(userId);
    }

    [Authorize]
    [HttpPost]
    public async Task<PostTodoResponse> PostTodo([FromBody] PostTodoRequest todoData)
    {
        string? userId = User.FindFirst("UserId")?.Value;
        return await _todoService.AddTodoAsync(userId, todoData);
    }

    [HttpDelete]
    public ActionResult<string> RemoveTodo([FromBody] DeleteTodoRequest req){
        bool success = _todoService.DeleteTodo(req.Id);
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
