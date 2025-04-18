using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.DTOs.TodoDTOs;
using Microsoft.AspNetCore.Authorization;
using TodoApi.Common;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : CustomControllerBase
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
        return await _todoService.GetTodosByUserAsync(this.GetUserId());
    }

    [Authorize]
    [HttpPost]
    public async Task<PostTodoResponse> PostTodo([FromBody] PostTodoRequest todoData)
    {
        string? userId = User.FindFirst("UserId")?.Value;
        return await _todoService.AddTodoAsync(this.GetUserId(), todoData);
    }

    [Authorize]
    [HttpDelete]
    public ActionResult<string> RemoveTodo([FromBody] DeleteTodoRequest req){
        bool success = _todoService.DeleteTodo(req.Id);

        string? userId =this.GetUserId();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        bool success = _todoService.DeleteTodo(userId, req.Id);

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
