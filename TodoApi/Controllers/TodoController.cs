using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.DTOs.TodoDTOs;
using Microsoft.AspNetCore.Authorization;
using TodoApi.Common;
using System.Threading.Tasks;
using TodoApi.DTOs.ApiResponse;

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
        return await _todoService.GetTodosByUserAsync(this.GetUserId());
    }

    [Authorize]
    [HttpPost]
    public async Task<PostTodoResponse> PostTodo([FromBody] PostTodoRequest todoData)
    {
        return await _todoService.AddTodoAsync(this.GetUserId(), todoData);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> RemoveTodo([FromBody] DeleteTodoRequest req)
    {
        bool success = await _todoService.DeleteTodoAsync(this.GetUserId(), req.Id);

        ApiResponse apiResponse = new(){SuccessStatus=success};

        if (success) return Ok(apiResponse);

        apiResponse.MessageFromServer = "Todo not found!";
        return BadRequest(apiResponse);
    }

    [HttpPut]
    public async Task<ActionResult> PutTodo([FromBody] Todo new_todo){
        bool success = await _todoService.DeleteTodoAsync(this.GetUserId(), req.Id);

        ApiResponse apiResponse = new(){SuccessStatus=success};

        if (success) return Ok(apiResponse);

        apiResponse.MessageFromServer = "Todo not found!";
        return BadRequest(apiResponse);
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateTodo([FromBody] UpdateTodoRequest data){

        bool success = await _todoService.UpdateTodoAsync(this.GetUserId(), data);

        ApiResponse apiResponse = new(){SuccessStatus=success};

        if (success) return Ok(apiResponse);

        apiResponse.MessageFromServer = "Todo not found!";
        return BadRequest(apiResponse);
    }
}